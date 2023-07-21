using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SubscribeController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public SubscribeController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            IEnumerable<Subscribe> subscribes= await _context.Subscribes.Where(x=>!x.IsDeleted).ToListAsync();
            return View(subscribes);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Subscribe? Subscribe = await _context.Subscribes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Subscribe == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Subscribe.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Subscribe");
        }
    }
}
