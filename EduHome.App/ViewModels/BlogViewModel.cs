using EduHome.Core.Entities;

namespace EduHome.App.ViewModels
{
    public class BlogViewModel
    {
        public List<Blog>? Blogs;
        public List<Tag>? Tags;
        public List<Category>? Categories;
        public Blog? Blog { get; set; }
        public Message? Message { get; set; }
    }
}
