﻿
using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SocialNetworkController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public SocialNetworkController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int TotalCount = _context.SocialNetworks.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            IEnumerable<SocialNetwork> socialNetworks= await _context.SocialNetworks.Include(x=>x.Teacher).Where(x=>!x.IsDeleted).Skip((page - 1) * 4).Take(4).ToListAsync();
            return View(socialNetworks);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialNetwork socialNetwork)
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(socialNetwork);
            }

            if (socialNetwork.Icon == null)
            {
                ModelState.AddModelError("Icon", "Icon is reqired");
                return View(socialNetwork);
            }

            socialNetwork.CreatedAt = DateTime.Now;
          await  _context.SocialNetworks.AddAsync(socialNetwork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SocialNetwork? socialNetwork=await _context.SocialNetworks.Where(x=>!x.IsDeleted &&  x.Id == id).FirstOrDefaultAsync();
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (socialNetwork==null)
            {
                return NotFound();
            }
           return View(socialNetwork);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SocialNetwork updatesocialNetwork)
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            SocialNetwork? socialNetwork = await _context.SocialNetworks.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if(socialNetwork == null)
            {
                return NotFound();
            }


			if (socialNetwork.Icon == null)
			{
				ModelState.AddModelError("Icon", "Icon is reqired");
				return View(socialNetwork);
			}

			socialNetwork.Icon=updatesocialNetwork.Icon;
            socialNetwork.Link=updatesocialNetwork.Link;
            socialNetwork.UpdatedAt = DateTime.Now;
    
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SocialNetwork socialNetwork=await _context.SocialNetworks.Where(x=>!x.IsDeleted && x.Id==id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            if (socialNetwork == null)
            {
                return NotFound();
            }

            socialNetwork.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
