using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutWelcomeController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AboutWelcomeController(EduHomeAppDxbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AboutWelcome> AboutWelcomes = await _context.AboutWelcomes.Where(x => !x.IsDeleted).ToListAsync();
            return View(AboutWelcomes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AboutWelcome AboutWelcome)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (AboutWelcome.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "File must be choosen");
            }

            //if (!Helper.IsImage(AboutWelcome.FormFile))
            //{
            //    ModelState.AddModelError("FileForm", "File type must be image");
            //    return View();
            //}

            if (!Helper.IsSizeOk(AboutWelcome.FormFile, 1))
            {
                ModelState.AddModelError("FileForm", "File size must be less than 1mb");
                return View();
            }


            AboutWelcome.Image = AboutWelcome.FormFile?.createimage(_environment.WebRootPath, "assets/img/about/");
            AboutWelcome.CreatedAt = DateTime.Now;
            await _context.AboutWelcomes.AddAsync(AboutWelcome);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            AboutWelcome? AboutWelcome = _context.AboutWelcomes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (AboutWelcome == null)
            {
                return NotFound();
            }
            return View(AboutWelcome);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AboutWelcome updateAboutWelcome, int id)
        {
            AboutWelcome? AboutWelcome = await _context.AboutWelcomes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (AboutWelcome == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(AboutWelcome);
            }

            if (AboutWelcome.FormFile != null)
            {

                //if (!Helper.IsImage(AboutWelcome.FormFile))
                //{
                //    ModelState.AddModelError("FileForm", "File type must be image");
                //    return View();
                //}

                if (!Helper.IsSizeOk(AboutWelcome.FormFile, 1))
                {
                    ModelState.AddModelError("FileForm", "File size must be less than 1mb");
                    return View();
                }

                Helper.removeimage(_environment.WebRootPath, "assets/img/about/", AboutWelcome.Image);

                AboutWelcome.Image = updateAboutWelcome.FormFile.createimage(_environment.WebRootPath, "assets/img/about/");
            }

            AboutWelcome.Title = updateAboutWelcome.Title;
            AboutWelcome.UpdatedAt = DateTime.Now;
            AboutWelcome.Link = updateAboutWelcome.Link;
            AboutWelcome.Description1 = updateAboutWelcome.Description1;
            AboutWelcome.Description2= updateAboutWelcome.Description2;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AboutWelcome? AboutWelcome = await _context.AboutWelcomes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (AboutWelcome == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            AboutWelcome.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
