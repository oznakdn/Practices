using DefaultCrud_AjaxCrud_ImageUpload.Data;
using DefaultCrud_AjaxCrud_ImageUpload.PasswordSaltHash;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace DefaultCrud_AjaxCrud_ImageUpload
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("AppSqliteConnection"));
                options.UseLazyLoadingProxies();

            });

            builder.Services.AddScoped<IPasswordGenerator, PasswordGenerator>();

            builder.Services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(opts =>
               {
                   opts.Cookie.Name = ".WebApplication2.auth";
                   opts.ExpireTimeSpan = TimeSpan.FromDays(7);
                   opts.SlidingExpiration = false;
                   opts.LoginPath = "/Account/Login";
                   opts.LogoutPath = "/Account/Logout";
                   opts.AccessDeniedPath = "/Home/AccessDenied";
               });


            var app = builder.Build();




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}