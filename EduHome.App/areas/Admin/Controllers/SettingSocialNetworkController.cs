
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
    public class SettingSocialNetworkController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public SettingSocialNetworkController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<SettingSocialNetwork> SettingSocialNetworks= await _context.SettingSocialNetworks.Include(x=>x.Setting).Where(x=>!x.IsDeleted).ToListAsync();
            return View(SettingSocialNetworks);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Icon,string Link)
        {
            ViewBag.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            Setting setting = _context.Settings.Where(x => !x.IsDeleted).FirstOrDefault();
          await  _context.SettingSocialNetworks.AddAsync(new SettingSocialNetwork { Icon=Icon,Link=Link,CreatedAt=DateTime.Now,Setting=setting});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SettingSocialNetwork? SettingSocialNetwork=await _context.SettingSocialNetworks.Where(x=>!x.IsDeleted &&  x.Id == id).FirstOrDefaultAsync();
            ViewBag.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();
            if (SettingSocialNetwork==null)
            {
                return NotFound();
            }
           return View(SettingSocialNetwork);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SettingSocialNetwork updateSettingSocialNetwork)
        {
            ViewBag.Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();
            SettingSocialNetwork? SettingSocialNetwork = await _context.SettingSocialNetworks.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if(SettingSocialNetwork == null)
            {
                return NotFound();
            }

            SettingSocialNetwork.Icon=updateSettingSocialNetwork.Icon;
            SettingSocialNetwork.Link=updateSettingSocialNetwork.Link;
            SettingSocialNetwork.UpdatedAt = DateTime.Now;
    
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SettingSocialNetwork SettingSocialNetwork=await _context.SettingSocialNetworks.Where(x=>!x.IsDeleted && x.Id==id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            if (SettingSocialNetwork == null)
            {
                return NotFound();
            }

            SettingSocialNetwork.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
