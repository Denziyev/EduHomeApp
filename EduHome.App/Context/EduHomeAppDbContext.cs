using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeAppDxbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public EduHomeAppDxbContext(DbContextOptions<EduHomeAppDxbContext> options) : base(options)
        {

        }
    }
}
