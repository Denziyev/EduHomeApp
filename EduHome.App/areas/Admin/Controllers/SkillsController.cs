﻿
using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SkillsController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public SkillsController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int TotalCount = _context.Skills.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            ViewBag.CurrentPage = page;

            IEnumerable<Skills> Skills = await _context.Skills.Include(x => x.Teacher).Where(x => !x.IsDeleted).Skip((page - 1) * 4).Take(4).ToListAsync();
            return View(Skills);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Skills Skills)
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            Skills.CreatedAt = DateTime.Now;
            await _context.Skills.AddAsync(Skills);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Skills? Skills = await _context.Skills.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if (Skills == null)
            {
                return NotFound();
            }
            return View(Skills);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Skills updateSkills)
        {
            ViewBag.Teachers = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            Skills? Skills = await _context.Skills.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (Skills == null)
            {
                return NotFound();
            }

            Skills.Name = updateSkills.Name;
            Skills.Percent = updateSkills.Percent;
            Skills.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Skills? Skills = await _context.Skills.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (Skills == null)
            {
                return NotFound();
            }

            Skills.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}

