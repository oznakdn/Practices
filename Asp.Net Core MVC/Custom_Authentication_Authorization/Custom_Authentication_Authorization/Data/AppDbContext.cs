using Custom_Authentication_Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Custom_Authentication_Authorization.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
