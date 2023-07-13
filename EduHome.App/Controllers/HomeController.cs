
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
				sliders = await _context.Sliders.Where(x => !x.IsDeleted).ToListAsync()
			};
			return View(homeViewModel);
		}
	}
}