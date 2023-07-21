using EduHome.App.Context;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UserController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserController(EduHomeAppDxbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
         
        }

        public IActionResult Index()
        {
            List < AppUser > users= _context.Users.ToList();
            return View(users);
        }

        public async Task<string> UserStatus(string id)
        {
            AppUser user= await _userManager.FindByIdAsync(id);
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return "Admin";
            }
            else if(await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                return "SuperAdmin";
            }
            else
            {
                return "User";
            }
        }

        
    }
}
