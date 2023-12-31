﻿
using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
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
                settings = await _context.Settings.Where(x => !x.IsDeleted).FirstOrDefaultAsync(),
                teachers = await _context.Teachers.Where(x => !x.IsDeleted).Include(x => x.Position).Where(x => !x.IsDeleted).
                Include(x => x.SocialNetworks.Where(x => !x.IsDeleted)).
                Include(x => x.Faculty).Where(x => !x.IsDeleted).
                Include(x => x.Skills.Where(x => !x.IsDeleted)).ToListAsync(),
                noticeboards=_context.NoticeBoards.Where(x=>!x.IsDeleted).ToList(),
                blogs = await _context.Blogs.Where(x => !x.IsDeleted).Include(x => x.Category).Where(x => !x.IsDeleted).Include(x => x.BlogTags.Where(x => !x.IsDeleted)).ToListAsync(),

                subscribes = await _context.Subscribes.Where(x => !x.IsDeleted).ToListAsync(),
                courses = await _context.Courses.Where(x=>!x.IsDeleted).Include(x=>x.Category).Where(x => !x.IsDeleted).Include(x=>x.CourseTags.Where(x => !x.IsDeleted)).Include(x=>x.Feature).ToListAsync(),
            };

            
			return View(homeViewModel);
		}
        [HttpPost]
        public async Task<IActionResult> SendEmail(string email)
        {
            if (email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(await _context.Subscribes.Where(x=>!x.IsDeleted).AnyAsync(x=>x.Email== email))
            {
                TempData["SendEmailno"] = "This email is already subscribed";
                return RedirectToAction("Index", "Home");
            }
            _context.Subscribes?.AddAsync(new Subscribe {  Email = email,CreatedAt=DateTime.Now});
            _context.SaveChanges();
            TempData["SendEmail"] = "Subscribed succesfully";
            return RedirectToAction("Index","Home");   
            
        }
	}
}