using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> sliders { get; set; }
 
        public Setting settings { get; set; }

        public List<Teacher> teachers { get; set; }

      public List<Course> courses { get; set; }
        public List<Blog> blogs { get; set; }

        public List<Subscribe> subscribes { get; set; }
        public List<NoticeBoard> noticeboards { get; set; }
    }
}
