using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SettingController(EduHomeAppDxbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Setting> Settings = await _context.Settings.Where(x => !x.IsDeleted).ToListAsync();
            return View(Settings);
        }

        //[HttpGet]
        //public async Task<IActionResult> Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Setting Setting)
        //{
        //    if (!ModelState.IsValid )
        //    {
        //        return View();
        //    }
        //    if (Setting.LogoFormFile == null || Setting.MailIconFormFile==null || Setting.PhoneIconFormFile==null ||  Setting.AdressIconFormFile==null || Setting.welcomeImageFormFile==null)
        //    {
        //        ModelState.AddModelError("FormFile", "File must be choosen");
        //    }

        //    //if (!(Helper.IsImage(Setting.LogoFormFile) && Helper.IsImage(Setting.MailIconFormFile) && Helper.IsImage(Setting.PhoneIconFormFile) && Helper.IsImage(Setting.AdressIconFormFile) && Helper.IsImage(Setting.welcomeImageFormFile)))
        //    //{
        //    //    ModelState.AddModelError("FileForm", "File type must be image");
        //    //    return View();
        //    //}

        //    if (!(Helper.IsSizeOk(Setting.LogoFormFile, 1)&& Helper.IsSizeOk(Setting.MailIconFormFile, 1)&& Helper.IsSizeOk(Setting.PhoneIconFormFile, 1)&& Helper.IsSizeOk(Setting.AdressIconFormFile, 1)&& Helper.IsSizeOk(Setting.welcomeImageFormFile, 1)))
        //    {
        //        ModelState.AddModelError("FileForm", "File size must be less than 1mb");
        //        return View();
        //    }


        //    Setting.WelcomeImage = Setting.welcomeImageFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
        //    Setting.IconAdress = Setting.AdressIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
        //    Setting.IconMail = Setting.MailIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
        //    Setting.IconPhone = Setting.PhoneIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
        //    Setting.Logo = Setting.LogoFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
        //    Setting.CreatedAt = DateTime.Now;
        //    await _context.Settings.AddAsync(Setting);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Setting? Setting = _context.Settings.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (Setting == null)
            {
                return NotFound();
            }
            return View(Setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Setting updateSetting, int id)
        {
            Setting? Setting = await _context.Settings.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Setting == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(Setting);
            }

            if (updateSetting.LogoFormFile == null || updateSetting.MailIconFormFile == null ||
                updateSetting.PhoneIconFormFile == null || updateSetting.AdressIconFormFile == null || updateSetting.welcomeImageFormFile == null)
            {
                ModelState.AddModelError("FormFile", "File must be choosen");
            }
            else
            {



                //if (!(Helper.IsImage(Setting.LogoFormFile) && Helper.IsImage(Setting.MailIconFormFile) && Helper.IsImage(Setting.PhoneIconFormFile) && Helper.IsImage(Setting.AdressIconFormFile) && Helper.IsImage(Setting.welcomeImageFormFile)))
                //{
                //    ModelState.AddModelError("FileForm", "File type must be image");
                //    return View();
                //}

                if (!(Helper.IsSizeOk(updateSetting.LogoFormFile, 1) && Helper.IsSizeOk(updateSetting.MailIconFormFile, 1) && Helper.IsSizeOk(updateSetting.PhoneIconFormFile, 1) && Helper.IsSizeOk(updateSetting.AdressIconFormFile, 1) && Helper.IsSizeOk(updateSetting.welcomeImageFormFile, 1)))
                {
                    ModelState.AddModelError("FileForm", "File size must be less than 1mb");
                    return View();
                }
                Helper.removeimage(_environment.WebRootPath, "assets/img/footer/", Setting.WelcomeImage);
                Helper.removeimage(_environment.WebRootPath, "assets/img/footer/", Setting.IconAdress);
                Helper.removeimage(_environment.WebRootPath, "assets/img/footer/", Setting.IconMail);
                Helper.removeimage(_environment.WebRootPath, "assets/img/footer/", Setting.IconPhone);
                Helper.removeimage(_environment.WebRootPath, "assets/img/footer/", Setting.Logo);
                Setting.WelcomeImage = updateSetting.welcomeImageFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
                Setting.IconAdress = updateSetting.AdressIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
                Setting.IconMail = updateSetting.MailIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
                Setting.IconPhone = updateSetting.PhoneIconFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");
                Setting.Logo = updateSetting.LogoFormFile?.createimage(_environment.WebRootPath, "assets/img/footer/");




            }


            Setting.NavbarTitle = updateSetting.NavbarTitle;
            Setting.MainPhone = updateSetting.MainPhone;
            Setting.DescriptionFooter = updateSetting.DescriptionFooter;
            Setting.CityAdress = updateSetting.CityAdress;
            Setting.StreetAdress = updateSetting.StreetAdress;
            Setting.Phone1 = updateSetting.Phone1;
            Setting.Phone2 = updateSetting.Phone2;
            Setting.WelcomeTittle = updateSetting.WelcomeTittle;
            Setting.WelcomeDesc1 = updateSetting.WelcomeDesc1;
            Setting.WelcomeDesc2 = updateSetting.WelcomeDesc2;
            Setting.WelcomeButtonContent = updateSetting.WelcomeButtonContent;
            Setting.WelcomeButtonLink = updateSetting.WelcomeButtonLink;
            Setting.NoticeVideoLink = updateSetting.NoticeVideoLink;
            Setting.NoticeText = updateSetting.NoticeText;
            Setting.MailFooter = updateSetting.MailFooter;
            Setting.SiteFooter = updateSetting.SiteFooter;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Setting? Setting = await _context.Settings.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (Setting == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Setting.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
