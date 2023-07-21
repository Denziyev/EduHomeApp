using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public ContactController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ContactViewModel contactViewModel = new ContactViewModel()
            {
                Settings = _context.Settings.Where(x => !x.IsDeleted).FirstOrDefault(),
                
            };
            return View(contactViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string name,string email,string subject,string message)
        {
            if (name == null || subject == null || message == null || email == null)
            {
                return RedirectToAction("index", "contact");

            }

            _context.Messages.Add(new Message { Name=name,Email=email,message=message,Subject=subject});
            TempData["SendMessage"] = "Message is sended";
            _context.SaveChanges();
            return RedirectToAction("index","contact");
        }
    }
}
