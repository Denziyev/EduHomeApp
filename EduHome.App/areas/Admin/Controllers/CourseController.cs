using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMailService _mailService;

        public CourseController(EduHomeAppDxbContext context, IWebHostEnvironment env, IMailService mailService)
        {
            _context = context;
            _env = env;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> Courses = await _context.Courses.Where(x => !x.IsDeleted).Include(x=>x.Category).ToListAsync();
            return View(Courses);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course Course)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(Course);
            }

            if (Course.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Image can not be null");
                return View(Course);
            }
            if (Course == null)
            {
                ModelState.AddModelError("Course", "Course  is required");
                return View();
            }

            Course.Image = Course.FormFile.createimage(_env.WebRootPath, "assets/img/course/");
            Course.CreatedAt = DateTime.Now;

            foreach (var item in Course.TagIds)
            {
                if (!await _context.Tags.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Valid item");
                    return View(Course);
                }
                CourseTag CourseTag = new CourseTag
                {
                    TagId = item,
                    Course = Course,
                    CreatedAt = DateTime.Now
                };
                await _context.CourseTags.AddAsync(CourseTag);
            }


            //email gondermek yenilik haqqinda
            List<Subscribe> subscribers = await _context.Subscribes.Where(x => !x.IsDeleted).ToListAsync();

            foreach (var item in subscribers)
            {
                //string token = await _userManager.GeneratePasswordResetTokenAsync(item);

                UriBuilder uriBuilder = new UriBuilder();

                var link = Url.Action(action: "index", controller: "Home",
                    values: new { email = item.Email },
                    protocol: Request.Scheme);

                await _mailService.Send("ilkinhd@code.edu.az", item.Email, link, "New Course", "Click me for New Course");
            }

            await _context.Courses.AddAsync(Course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
         

            Course? Course = await _context.Courses.
                Where(x => !x.IsDeleted && x.Id == id).
                Include(x => x.Feature).Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.CourseTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                FirstOrDefaultAsync();

            return View(Course);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Course updatecourse)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            Course? Course = await _context.Courses.
                Where(x => !x.IsDeleted && x.Id == id).
                Include(x => x.Feature).Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.CourseTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                FirstOrDefaultAsync();

            if (Course == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(Course);
            }

            List<CourseTag> RemovableTag = await _context.CourseTags.
                Where(x => !updatecourse.TagIds.Contains(x.TagId))
                .ToListAsync();

            _context.CourseTags.RemoveRange(RemovableTag);

            foreach (var item in updatecourse.TagIds)
            {
                if (_context.CourseTags.Where(x => x.CourseId == id &&
                   x.TagId == item).Count() > 0)
                {
                    continue;
                }
                else
                {
                    await _context.CourseTags.AddAsync(new CourseTag
                    {
                        CourseId = id,
                        TagId = item
                    });
                }

            }


            Helper.removeimage(_env.WebRootPath, "assets/img/course", Course.Image);
            Course.Image = updatecourse.FormFile?.createimage(_env.WebRootPath, "assets/img/course");

            Course.Name = updatecourse.Name;
            Course.Description = updatecourse.Description;
            Course.Feature = updatecourse.Feature;
            Course.Abouttext= updatecourse.Abouttext;
            Course.Applytext = updatecourse.Applytext;
            Course.Certification = updatecourse.Certification;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Course? Course = await
                _context.Courses.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (Course == null)
            {
                return NotFound();
            }
            Course.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
