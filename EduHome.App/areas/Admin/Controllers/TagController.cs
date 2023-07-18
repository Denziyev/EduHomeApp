
using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]

    public class TagController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public TagController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            return View(Tags);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag Tag)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag.CreatedAt = DateTime.Now;
            await _context.Tags.AddAsync(Tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Tag? Tag = await _context.Tags.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }
            return View(Tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Tag postTag, int id)
        {
            Tag? Tag = await _context.Tags.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag.Name = postTag.Name;
            Tag.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Tag? Tag = await _context.Tags.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Tag == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
