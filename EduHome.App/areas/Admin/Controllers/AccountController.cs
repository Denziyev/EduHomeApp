﻿using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {

        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(EduHomeAppDxbContext context, IWebHostEnvironment environment, UserManager<AppUser> userManager, SignInManager<AppUser> signinManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
            _signinManager = signinManager;
        }
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AdminCreate()
        {
            AppUser SuperAdmin = new AppUser
            {
                Name = "SuperAdmin",
                Surname = "SuperAdmin",
                Email = "SuperAdmin@Mail.ru",
                UserName = "SuperAdmin",
            
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(SuperAdmin, "Admin123@");

            AppUser Admin = new AppUser
            {
                Name = "Admin",
                Surname = "Admin",
                Email = "Admin@Mail.ru",
                UserName = "Admin",
        
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(Admin, "Admin123@");

            await _userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");
            await _userManager.AddToRoleAsync(Admin, "Admin");
            return Json("ok");
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            AppUser appUser = await _userManager.FindByNameAsync(login.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View(login);
            }
            Microsoft.AspNetCore.Identity.SignInResult result =
                await _signinManager.PasswordSignInAsync(appUser, login.Password, login.isRememberMe, true);

            var role = await _userManager.GetRolesAsync(appUser);
            foreach (var roles in role)
            {
                if (roles == "User")
                {
                    ModelState.AddModelError("", "Wrong!");
                    return View();
                }

            }

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is blocked for 5 minute");
                    return View(login);
                }
                ModelState.AddModelError("", "Username or password incorrect");
                return View(login);
            }
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("index","home", new { area = "Default" });
        }






    }
}