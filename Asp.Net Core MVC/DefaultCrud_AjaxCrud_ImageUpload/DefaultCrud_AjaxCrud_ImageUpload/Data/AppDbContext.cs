using DefaultCrud_AjaxCrud_ImageUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace DefaultCrud_AjaxCrud_ImageUpload.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
