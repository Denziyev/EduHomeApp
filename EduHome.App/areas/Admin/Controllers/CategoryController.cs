
using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController:Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public CategoryController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        { 
            IEnumerable<Category> categories=await _context.Categories.Where(x=>!x.IsDeleted).ToListAsync();

            return View(categories);
        }

        [HttpGet] 
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            
            if(!ModelState.IsValid)
            {
                return View();
            }
            category.CreatedAt = DateTime.Now;
            await _context.Categories.AddAsync(category);
             await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? category=await _context.Categories.Where(x=>!x.IsDeleted &&  x.Id==id).FirstOrDefaultAsync();
            if(category==null)
            {
                return NotFound();
            }
           return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Category postcategory,int id)
        {
            Category? category = await _context.Categories.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
           category.Name = postcategory.Name;
            category.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            category.IsDeleted =true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }
    }
}
