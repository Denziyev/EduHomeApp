using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class AboutViewModel
    {
        public Setting settings { get; set; }
        public List<NoticeBoard> NoticeBoards { get; set; }
        public List<Teacher> teachers { get; set; }
    }
}
