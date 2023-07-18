using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public ContactController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            IEnumerable<Message> messages = await _context.Messages.Where(x => !x.IsDeleted).ToListAsync();
            return View(messages);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Message? message = await _context.Messages.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (message == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Contact");
        }
    }
}
