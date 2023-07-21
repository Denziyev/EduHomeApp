using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BlogController : Controller
    {
        private readonly EduHomeAppDxbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IMailService _mailService;



        public BlogController(EduHomeAppDxbContext context, IWebHostEnvironment env, IMailService mailService)
        {
            _context = context;
            _env = env;
            _mailService = mailService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> Blogs = await _context.Blogs.Where(x => !x.IsDeleted).Include(x=>x.Category).ToListAsync();
            return View(Blogs);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog Blog)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(Blog);
            }

            if (Blog == null)
            {
                ModelState.AddModelError("Blog", "Blog  is required");
                return View(Blog);
            }

            if (Blog.Time == null)
            {
				ModelState.AddModelError("Time", "Time  is required");
				return View(Blog);
			}

			if (Blog.FormFile == null)
			{
				ModelState.AddModelError("FormFile", "File must be choosen");
				return View(Blog);
			}

			if (!Helper.IsImage(Blog.FormFile))
			{
				ModelState.AddModelError("FormFile", "File type must be image");
				return View(Blog);
			}

			if (!Helper.IsSizeOk(Blog.FormFile, 1))
			{
				ModelState.AddModelError("FormFile", "File size must be less than 1mb");
				return View(Blog);
			}

			Blog.Image = Blog.FormFile.createimage(_env.WebRootPath, "assets/img/Blog/");
            Blog.CreatedAt = DateTime.Now;

            foreach (var item in Blog.TagIds)
            {
                if (!await _context.Tags.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Valid item");
                    return View(Blog);
                }
                BlogTag BlogTag = new BlogTag
                {
                    TagId = item,
                    Blog = Blog,
                    CreatedAt = DateTime.Now
                };
                await _context.BlogTags.AddAsync(BlogTag);
            }

            //email gondermek yenilik haqqinda
            List<Subscribe> subscribers = await _context.Subscribes.Where(x => !x.IsDeleted).ToListAsync();

            foreach(var item in subscribers)
            {


                UriBuilder uriBuilder = new UriBuilder();

                var link = Url.Action(action: "index", controller: "Home",
                    values: new { email = item.Email },
                    protocol: Request.Scheme);

                await _mailService.Send("ilkinhd@code.edu.az", item.Email, link, "New Blog","Click me for New Blog");
            }


            await _context.Blogs.AddAsync(Blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
           


            Blog? Blog = await _context.Blogs.
                Where(x => !x.IsDeleted && x.Id == id).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                FirstOrDefaultAsync();

            return View(Blog);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog updateBlog)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            Blog? Blog = await _context.Blogs.
                Where(x => !x.IsDeleted && x.Id == id).
                Include(x => x.Category).Where(x => !x.IsDeleted).
                Include(x => x.BlogTags.Where(x => !x.IsDeleted)).
                ThenInclude(x => x.Tag).
                FirstOrDefaultAsync();

            if (Blog == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(Blog);
            }
			if (updateBlog.FormFile == null)
			{
				ModelState.AddModelError("FormFile", "File must be choosen");
				return View(updateBlog);
			}

			if (!Helper.IsImage(updateBlog.FormFile))
			{
				ModelState.AddModelError("FormFile", "File type must be image");
				return View(updateBlog);
			}

			if (!Helper.IsSizeOk(updateBlog.FormFile, 1))
			{
				ModelState.AddModelError("FormFile", "File size must be less than 1mb");
				return View(updateBlog);
			}

			List<BlogTag> RemovableTag = await _context.BlogTags.
                Where(x => !updateBlog.TagIds.Contains(x.TagId))
                .ToListAsync();

            _context.BlogTags.RemoveRange(RemovableTag);

            foreach (var item in updateBlog.TagIds)
            {
                if (_context.BlogTags.Where(x => x.BlogId == id &&
                   x.TagId == item).Count() > 0)
                {
                    continue;
                }
                else
                {
                    await _context.BlogTags.AddAsync(new BlogTag
                    {
                        BlogId = id,
                        TagId = item
                    });
                }

            }
           

            Helper.removeimage(_env.WebRootPath, "assets/img/blog", Blog.Image);
            Blog.Image = updateBlog.FormFile?.createimage(_env.WebRootPath, "assets/img/blog");

            Blog.Writer = updateBlog.Writer;
            Blog.Content= updateBlog.Content;
            Blog.Title= updateBlog.Title;
            Blog.CategoryId = updateBlog.CategoryId;
            Blog.UpdatedAt=DateTime.Now;
           
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Blog? Blog = await
                _context.Blogs.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (Blog == null)
            {
                return NotFound();
            }
            Blog.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
