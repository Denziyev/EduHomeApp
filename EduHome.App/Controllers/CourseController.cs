using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class CourseController : Controller
    {
        private readonly EduHomeAppDxbContext _context;

        public CourseController(EduHomeAppDxbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1,int id=0)
        {
            //int TotalCount=_context.Courses.Where(x=>!x.IsDeleted).Count();
            //ViewBag.TotalPage= (int)Math.Ceiling((decimal)TotalCount / 2);
            CourseViewModel courseViewModel = new CourseViewModel();

            if (id != 0)
            {
                courseViewModel.Courses = _context.Courses.Where(x => !x.IsDeleted && x.CategoryId==id).
              Include(x => x.Feature).Where(x => !x.IsDeleted).
              Include(x => x.Category).Where(x => !x.IsDeleted).
              Include(x => x.CourseTags.Where(x => !x.IsDeleted)).
              ThenInclude(x => x.Tag)./*Skip((page - 1) * 2).Take(2).*/
              ToList();
            }
            else
            {
                courseViewModel.Courses = _context.Courses.Where(x => !x.IsDeleted).
                Include(x => x.Feature).Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.CourseTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag)./*Skip((page - 1) * 2).Take(2).*/
                ToList();
            }

            
            courseViewModel.Tags = _context.Tags.Where(x => !x.IsDeleted).ToList();
                courseViewModel.Categories = _context.Categories.Where(x => !x.IsDeleted).ToList();
                courseViewModel.Blogs = _context.Blogs.Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                ToList();

            
            return View(courseViewModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            CourseViewModel CourseViewModel = new CourseViewModel
            {
                Course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id)
                      .Include(x => x.Feature).Where(x => !x.IsDeleted)
                        .Include(x => x.CourseTags.Where(x => !x.IsDeleted))
                        .Include(x => x.Category).Where(x => !x.IsDeleted)
                        .FirstOrDefaultAsync(),
                Courses = _context.Courses.Where(x => !x.IsDeleted).
                Include(x => x.Feature).Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.CourseTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                ToList(),
                Tags = _context.Tags.Where(x => !x.IsDeleted).ToList(),
                Categories = _context.Categories.Where(x => !x.IsDeleted).ToList(),
                Blogs = _context.Blogs.Where(x => !x.IsDeleted).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                ToList(),
            };

            if (CourseViewModel.Course == null)
            {
                return View(nameof(Index));
            }

            return View(CourseViewModel);
        }

        public async Task<IActionResult> Search(string search)
        {
            int TotalCount = _context.Courses.Where(x => !x.IsDeleted && (x.Name.Trim().ToLower().Contains(search.Trim().ToLower()) || x.Description.Trim().ToLower().Contains(search.Trim().ToLower()))).Count();

            List<Course> courses = await _context.Courses.Where(x => !x.IsDeleted &&( x.Name.Trim().ToLower().Contains(search.Trim().ToLower()) || x.Description.Trim().ToLower().Contains(search.Trim().ToLower())))
                .Include(x => x.Feature).Where(x=>!x.IsDeleted)
                  .Include(x => x.Category)
                       .Include(x => x.CourseTags.Where(x => !x.IsDeleted))
                 .ThenInclude(x => x.Tag)
                .ToListAsync();
            return Json(courses);
        }


    }
}
