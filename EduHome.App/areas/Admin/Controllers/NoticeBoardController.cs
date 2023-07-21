
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
    public class NoticeBoardController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public NoticeBoardController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<NoticeBoard> NoticeBoards = await _context.NoticeBoards.Where(x => !x.IsDeleted).ToListAsync();

            return View(NoticeBoards);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticeBoard NoticeBoard)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            NoticeBoard.CreatedAt = DateTime.Now;
            await _context.NoticeBoards.AddAsync(NoticeBoard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            NoticeBoard? NoticeBoard = await _context.NoticeBoards.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (NoticeBoard == null)
            {
                return NotFound();
            }
            return View(NoticeBoard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(NoticeBoard postNoticeBoard, int id)
        {
            NoticeBoard? NoticeBoard = await _context.NoticeBoards.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (NoticeBoard == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            NoticeBoard.Content = postNoticeBoard.Content;
            NoticeBoard.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            NoticeBoard? NoticeBoard = await _context.NoticeBoards.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (NoticeBoard == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            NoticeBoard.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "NoticeBoard");
        }
    }
}
