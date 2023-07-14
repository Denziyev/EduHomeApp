
using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public PositionController(EduHomeAppDxbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<Position> positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            return View(positions);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            position.CreatedAt = DateTime.Now;
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Position? Position = await _context.Positions.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Position == null)
            {
                return NotFound();
            }
            return View(Position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Position postPosition, int id)
        {
            Position? Position = await _context.Positions.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Position == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Position.Name = postPosition.Name;
            Position.UpdatedAt   = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Position? Position = await _context.Positions.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Position == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Position.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Position");
        }

    }
}
