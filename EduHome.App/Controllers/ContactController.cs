﻿using EduHome.App.Context;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string name,string email,string subject,string message)
        {
           
            
            _context.Messages.Add(new Message { Name=name,Email=email,message=message,Subject=subject});
            _context.SaveChanges();
            return RedirectToAction("index","contact");
        }
    }
}