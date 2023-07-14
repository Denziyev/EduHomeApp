using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeAppDxbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AboutWelcome> AboutWelcomes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SocialNetwork> SocialNetworks { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Faculty> Faculties { get; set; }

		public EduHomeAppDxbContext(DbContextOptions<EduHomeAppDxbContext> options) : base(options)
        {

        }
    }
}
