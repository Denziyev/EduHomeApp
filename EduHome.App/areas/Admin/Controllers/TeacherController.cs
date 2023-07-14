
using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _environment;

        public TeacherController(EduHomeAppDxbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> Teachers = await _context.Teachers.Include(x => x.Position).
                Where(c => !c.IsDeleted).Include(x => x.Faculty).
				Where(c => !c.IsDeleted).ToListAsync();
            return View(Teachers);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Faculties = await _context.Faculties.Where(p => !p.IsDeleted).ToListAsync();

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher Teacher)
        {
            ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Faculties = await _context.Faculties.Where(p => !p.IsDeleted).ToListAsync();

			if (!ModelState.IsValid)
            {
                return View();
            }
            if (Teacher.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "File must be choosen");
            }

            //if (!Helper.IsImage(Teacher.FormFile))
            //{
            //    ModelState.AddModelError("FormFile", "File type must be image");
            //    return View();
            //}

            if (!Helper.IsSizeOk(Teacher.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "File size must be less than 1mb");
                return View();
            }
            Teacher.Image = Teacher.FormFile?.createimage(_environment.WebRootPath, "assets/img/teacher/");
            Teacher.CreatedAt = DateTime.Now;

            await _context.AddAsync(Teacher);

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "Teacher");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Positions = await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Faculties = await _context.Faculties.Where(p => !p.IsDeleted).ToListAsync();

			Teacher? Teacher = await _context.Teachers.
                  Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (Teacher == null)
            {
                return NotFound();
            }
            return View(Teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Teacher Teacher)
        {

            Teacher? UpdateTeacher = await _context.Teachers.
                Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (Teacher == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(UpdateTeacher);
            }

            if (Teacher.FormFile != null)
            {
                //if (!Helper.IsImage(Teacher.FormFile))
                //{
                //    ModelState.AddModelError("FormFile", "File size must be less than 1mb!");
                //    return View();
                //}
                if (!Helper.IsSizeOk(Teacher.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "File size must be less than 1mb!");
                    return View();
                }
                Helper.removeimage(_environment.WebRootPath, "assets/img/teacher/", UpdateTeacher.Image);
                UpdateTeacher.Image = Teacher.FormFile.createimage(_environment.WebRootPath, "assets/img/teacher/");
            }

            UpdateTeacher.FullName = Teacher.FullName;
            UpdateTeacher.Description = Teacher.Description;
            UpdateTeacher.PositionId = Teacher.PositionId;
            UpdateTeacher.SocialNetworks = Teacher.SocialNetworks;
            UpdateTeacher.Skills = Teacher.Skills;
            UpdateTeacher.Degree=Teacher.Degree;
            UpdateTeacher.Hobbies= Teacher.Hobbies;
            UpdateTeacher.Mail= Teacher.Mail;
            UpdateTeacher.Phone= Teacher.Phone;
            UpdateTeacher.Faculty= Teacher.Faculty;
            UpdateTeacher.Skype= Teacher.Skype;
            UpdateTeacher.Experience= Teacher.Experience;
            UpdateTeacher.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Teacher? Teacher = await _context.Teachers.
               Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (Teacher == null)
            {
                return NotFound();
            }

            Teacher.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
