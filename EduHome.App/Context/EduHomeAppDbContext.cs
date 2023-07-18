using EduHome.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EduHome.App.Context
{
    public class EduHomeAppDxbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Setting> Settings { get; set; }


        public EduHomeAppDxbContext(DbContextOptions<EduHomeAppDxbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(a => a.Feature)
                .WithOne(b => b.Course)
                .HasForeignKey<Feature>(b => b.CourseId);

            modelBuilder.Entity<Setting>()
        .HasMany(e => e.SocialNetworks)
        .WithOne(e => e.Setting)
        .HasForeignKey(e => e.SettingId)
        .IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }
}
