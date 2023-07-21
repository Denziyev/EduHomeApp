using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(EduHomeAppDxbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(x => !x.IsDeleted).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "File must be choosen");
                return View(slider);
            }

            if (!Helper.IsImage(slider.FormFile))
            {
                ModelState.AddModelError("FileForm", "File type must be image");
                return View();
            }

            if (!Helper.IsSizeOk(slider.FormFile, 1))
            {
                ModelState.AddModelError("FileForm", "File size must be less than 1mb");
                return View();
            }


            slider.Image = slider.FormFile?.createimage(_environment.WebRootPath, "assets/img/slider/");
            slider.CreatedAt = DateTime.Now;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slider? slider = _context.Sliders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Slider updateslider, int id)
        {
            Slider? slider = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (updateslider == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updateslider);
            }

            if (updateslider.FormFile != null)
            {

                if (!Helper.IsImage(updateslider.FormFile))
                {
                    ModelState.AddModelError("FileForm", "File type must be image");
                    return View();
                }

                if (!Helper.IsSizeOk(updateslider.FormFile, 1))
                {
                    ModelState.AddModelError("FileForm", "File size must be less than 1mb");
                    return View();
                }

                Helper.removeimage(_environment.WebRootPath, "assets/img/slider/", slider.Image);

                slider.Image = updateslider.FormFile.createimage(_environment.WebRootPath, "assets/img/slider/");
            }
            

            slider.Title = updateslider.Title;
            slider.UpdatedAt = DateTime.Now;
            slider.Link = updateslider.Link;
            slider.Description = updateslider.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Slider? slider = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (slider == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            slider.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
