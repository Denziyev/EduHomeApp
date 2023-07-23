using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public BlogController(EduHomeAppDxbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1,int id=0)
        {
            int TotalCount = _context.Blogs.Where(x => !x.IsDeleted).Count();
            ViewBag.TotalPage = (int)Math.Ceiling((decimal)TotalCount / 4);
            BlogViewModel BlogViewModel = new BlogViewModel();
            
                 if (id != 0)
            {
                BlogViewModel.Blogs = _context.Blogs.Where(x => !x.IsDeleted && x.CategoryId == id).
              Include(x => x.Category).Where(x => !x.IsDeleted).
              Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
              ThenInclude(x => x.Tag).Skip((page - 1) * 4).Take(4).
              ToList();
            }
            else
            {


                BlogViewModel.Blogs = _context.Blogs.Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).Skip((page - 1) * 4).Take(4).
                ToList();
            }
            BlogViewModel.Tags = _context.Tags.Where(x => !x.IsDeleted).ToList();
            BlogViewModel.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();

            
            return View(BlogViewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            BlogViewModel BlogViewModel = new BlogViewModel
            {
                Blog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
                        .Include(x => x.BlogTags.Where(x => !x.IsDeleted))
                        .Include(x => x.Category).Where(x => !x.IsDeleted)
                        .FirstOrDefaultAsync(),
                Blogs = _context.Blogs.Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                ToList(),
                Tags = _context.Tags.Where(x => !x.IsDeleted).ToList(),
                Categories = _context.Categories.Where(x => !x.IsDeleted).ToList(),

            };

            if (BlogViewModel.Blog == null)
            {
                return View(nameof(Index));
            }

            return View(BlogViewModel);
        }
    }
}
