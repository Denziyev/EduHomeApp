using EduHome.App.Context;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Services.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly EduHomeAppDxbContext _context;

        public SettingService(EduHomeAppDxbContext context)
        {
            _context = context;
        }
        public async Task<Setting?> Get()
        {
            Setting? setting = await _context.Settings.Where(x=>!x.IsDeleted).Include(x=>x.settingSocialNetworks).FirstOrDefaultAsync();
          
            return setting;
        }
    }
}
