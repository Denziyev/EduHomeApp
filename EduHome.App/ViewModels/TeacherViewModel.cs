using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class TeacherViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        public Teacher Teacher { get; set; }
    }
}
