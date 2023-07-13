
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduHome.App.Controllers
{
	public class HomeController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}