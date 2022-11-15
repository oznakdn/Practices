using AspNetCoreIdentity_Authentication_Authorization.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity_Authentication_Authorization.Data
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }
    }
}
