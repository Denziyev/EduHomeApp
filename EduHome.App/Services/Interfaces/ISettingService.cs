using EduHome.Core.Entities;

namespace EduHome.App.Services.Interfaces
{
    public interface ISettingService
    {
        public Task<Setting?> Get();
    }
}
