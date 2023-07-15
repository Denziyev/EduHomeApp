using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(EduHomeAppDxbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> Courses = await _context.Courses.Where(x => !x.IsDeleted).Include(x => x.Feature).ToListAsync();
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

            if (Course == null)
            {
                ModelState.AddModelError("Course", "Course  is required");
                return View();
            }

            Course.Image = Course.FormFile.createimage(_env.WebRootPath, "assets/img/course");
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
                Where(x => !Course.TagIds.Contains(x.TagId))
                .ToListAsync();

            _context.CourseTags.RemoveRange(RemovableTag);

            foreach (var item in Course.TagIds)
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



            Course.Image = updatecourse.FormFile?.createimage(_env.WebRootPath, "assets/img/course");

            _context.Courses.Update(Course);
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
