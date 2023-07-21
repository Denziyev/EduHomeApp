using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public TeacherController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index(int page=1)
        {
            int TotalCount = _context.Teachers.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 2);
            TeacherViewModel teacherViewModel = new TeacherViewModel
            {
                Teachers = await _context.Teachers.Where(x => !x.IsDeleted)
                      .Include(x => x.Faculty).Where(x => !x.IsDeleted)
                       .Include(x => x.Position).Where(x => !x.IsDeleted)
                        .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialNetworks.Where(x => !x.IsDeleted)).Skip((page - 1) * 2).Take(2)
                       .ToListAsync()
            };

            return View(teacherViewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            TeacherViewModel teacherViewModel = new TeacherViewModel
            {
                Teacher = await _context.Teachers.Where(x => !x.IsDeleted && x.Id == id)
                      .Include(x => x.Faculty).Where(x => !x.IsDeleted)
                       .Include(x => x.Position).Where(x => !x.IsDeleted)
                       .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialNetworks.Where(x => !x.IsDeleted))
                        .FirstOrDefaultAsync()
            };

            if(teacherViewModel.Teacher == null)
            {
                return View(nameof(Index));
            }

            return View(teacherViewModel);
        }
    }
}
