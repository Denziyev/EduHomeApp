using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Helpers
{
    public class UserStatusCheck
    {
        private readonly EduHomeAppDxbContext? _context;
        private readonly UserManager<AppUser> _userManager;

        //public static string UserStatus(string id)
        //{

        //    AppUser user =  _userManager.FindById(id);

        //    if ( _userManager.IsInRole(user, "Admin"))
        //    {
        //        return "Admin";
        //    }
        //    else if (_userManager.IsInRole(user, "SuperAdmin"))
        //    {
        //        return "SuperAdmin";
        //    }
        //    else
        //    {
        //        return "User";
        //    }
        //}
    }
}
