using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class FeatureController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public FeatureController(EduHomeAppDxbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {   
            IEnumerable<Feature> features =await _context.Features.Where(x=>!x.IsDeleted).Include(x=>x.Course).Where(x=>!x.IsDeleted).ToListAsync();
            return View(features);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted && x.Feature == null).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted && x.Feature==null).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            feature.CreatedAt = DateTime.Now;
            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();

            Feature? Feature = await _context.Features.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Feature == null)
            {
                return NotFound();
            }
            return View(Feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Feature postFeature, int id)
        {
            ViewBag.Courses = await _context.Courses.Where(x => !x.IsDeleted).ToListAsync();

            Feature? Feature = await _context.Features.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Feature == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Feature.CourseId = postFeature.CourseId;
            Feature.Duration = postFeature.Duration;
            Feature.ClassDuration=postFeature.ClassDuration;
            Feature.Assestments= postFeature.Assestments;
            Feature.Fee=postFeature.Fee;
            Feature.Language=postFeature.Language;
            Feature.StudentCount=postFeature.StudentCount;  
            Feature.Start=postFeature.Start;
            Feature.SkillLevel=postFeature.SkillLevel;
            Feature.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Feature? Feature = await _context.Features.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Feature == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Feature.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Feature");
        }
    }
}
