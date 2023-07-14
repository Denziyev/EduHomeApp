using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{

		[Area("Admin")]
		public class FacultyController : Controller
		{
			private readonly EduHomeAppDxbContext _context;

			public FacultyController(EduHomeAppDxbContext context)
			{
				_context = context;
			}


			public async Task<IActionResult> Index()
			{
				IEnumerable<Faculty> faculties = await _context.Faculties.Where(x => !x.IsDeleted).ToListAsync();
				return View(faculties);
			}
			[HttpGet]
			public async Task<IActionResult> Create()
			{

				return View();
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Create(Faculty Faculty)
			{
				if (!ModelState.IsValid)
				{
					return View();
				}
				Faculty.CreatedAt = DateTime.Now;
				await _context.Faculties.AddAsync(Faculty);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			[HttpGet]
			public async Task<IActionResult> Update(int id)
			{
				Faculty? Faculty = await _context.Faculties.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
				if (Faculty == null)
				{
					return NotFound();
				}
				return View(Faculty);
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Update(Faculty postFaculty, int id)
			{
				Faculty? Faculty = await _context.Faculties.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
				if (Faculty == null)
				{
					return NotFound();
				}
				if (!ModelState.IsValid)
				{
					return View();
				}
				Faculty.Name = postFaculty.Name;
				Faculty.UpdatedAt = DateTime.Now;
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			[HttpGet]
			public async Task<IActionResult> Delete(int id)
			{
				Faculty? Faculty = await _context.Faculties.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
				if (Faculty == null)
				{
					return NotFound();
				}
				if (!ModelState.IsValid)
				{
					return View();
				}
				Faculty.IsDeleted = true;
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Faculty");
			}

		}
	
}
