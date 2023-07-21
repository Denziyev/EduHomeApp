using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
	public class AboutController : Controller
	{
        private readonly EduHomeAppDxbContext _context;

        public AboutController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AboutViewModel aboutViewModel = new AboutViewModel()
            {
      
                teachers = await _context.Teachers.Where(x => !x.IsDeleted)
                      .Include(x => x.Faculty).Where(x => !x.IsDeleted)
                       .Include(x => x.Position).Where(x => !x.IsDeleted)
                        .Include(x => x.Skills.Where(x => !x.IsDeleted))
                        .Include(x => x.SocialNetworks.Where(x => !x.IsDeleted))
                       .ToListAsync(),
                NoticeBoards = _context.NoticeBoards.Where(x => !x.IsDeleted).ToList(),
                settings = await _context.Settings.Where(x => !x.IsDeleted).FirstOrDefaultAsync(),
            };
            return View(aboutViewModel);
        }
    }
}
