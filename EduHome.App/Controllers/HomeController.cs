
using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.App.Controllers
{
	public class HomeController : Controller
	{
		private readonly EduHomeAppDxbContext _context;

        public HomeController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
		{
			HomeViewModel homeViewModel = new HomeViewModel()
			{
				sliders = await _context.Sliders.Where(x => !x.IsDeleted).ToListAsync(),

                aboutWelcomes = await _context.AboutWelcomes.Where(x => !x.IsDeleted).ToListAsync(),
                teachers = await _context.Teachers.Where(x => !x.IsDeleted).Include(x => x.Position).Where(x => !x.IsDeleted).
                Include(x => x.SocialNetworks).Where(x => !x.IsDeleted).
                Include(x => x.Faculty).Where(x => !x.IsDeleted).
                Include(x => x.Skills).Where(x => !x.IsDeleted).ToListAsync()
            };
			return View(homeViewModel);
		}
	}
}