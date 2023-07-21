using EduHome.App.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
