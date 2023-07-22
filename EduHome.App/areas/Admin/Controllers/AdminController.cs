using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(EduHomeAppDxbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            List<AppUser> admins = new List<AppUser>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    admins.Add(user);
                }
            }
            return View(admins);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            AppUser Admin = new AppUser
            {
                Name = registerViewModel.Name,
                Surname = registerViewModel.Surname,
                Email = registerViewModel.Email,
                UserName = registerViewModel.UserName,
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(Admin, registerViewModel.Password);
            await _userManager.AddToRoleAsync(Admin, "Admin");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(string id)
        {
            var users = await _context.Users.ToListAsync();
            List<AppUser> Admins = new List<AppUser>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    Admins.Add(user);
                }
            }
            var updateuser = Admins.FirstOrDefault(x => x.Id == id);
            UserUpdateViewModel userUpdateViewModel = new UserUpdateViewModel()
            {
                UserName = updateuser.UserName,
                Email = updateuser.Email,
                Name = updateuser.Name,
                Surname = updateuser.Surname,
            };
            return View(userUpdateViewModel);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(string id, UserUpdateViewModel userUpdateViewModel)
        {
            var users = await _context.Users.ToListAsync();
            List<AppUser> admins = new List<AppUser>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    admins.Add(user);
                }
            }
            var updateuser = admins.FirstOrDefault(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                return View(userUpdateViewModel);
            }
            if (updateuser is null)
            {
                return NotFound();
            }
            updateuser.Name = userUpdateViewModel.Name;
            updateuser.Email = userUpdateViewModel.Email;
            updateuser.Surname = userUpdateViewModel.Surname;
            updateuser.UserName = userUpdateViewModel.UserName;
            var result = await _userManager.UpdateAsync(updateuser);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(userUpdateViewModel);
            }

            if (!string.IsNullOrWhiteSpace(userUpdateViewModel.NewPassword))
            {
                result = await _userManager.ChangePasswordAsync(updateuser, userUpdateViewModel.CurrentPassword, userUpdateViewModel.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(userUpdateViewModel);
                }

            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var users = await _context.Users.ToListAsync();
            List<AppUser> admins = new List<AppUser>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    admins.Add(user);
                }
            }
            var removeuser = admins.FirstOrDefault(x => x.Id == id);
            if (removeuser is null)
            {
                return NotFound();
            }
            _context.Users.Remove(removeuser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
