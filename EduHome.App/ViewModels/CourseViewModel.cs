using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class CourseViewModel
    {
        public List<Course>? Courses { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Blog>? Blogs { get; set; }
        public Message Message { get; set; }
        public Course? Course { get; set; }
    }
}
