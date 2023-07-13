using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeAppDxbContext : DbContext
    {
        public EduHomeAppDxbContext(DbContextOptions<EduHomeAppDxbContext> options) : base(options)
        {

        }
    }
}
