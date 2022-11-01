using Microsoft.EntityFrameworkCore;
using RedisCaching.Models;

namespace RedisCaching.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<Driver> Drivers { get; set; }

    }
}
