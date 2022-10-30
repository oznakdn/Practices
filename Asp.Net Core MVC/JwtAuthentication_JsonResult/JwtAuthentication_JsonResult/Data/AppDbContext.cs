using JwtAuthentication_JsonResult.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication_JsonResult.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
