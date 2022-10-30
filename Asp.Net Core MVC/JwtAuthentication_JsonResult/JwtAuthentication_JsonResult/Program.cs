using JwtAuthentication_JsonResult.Data;
using JwtAuthentication_JsonResult.Helpers;
using JwtAuthentication_JsonResult.PasswordSaltHash;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace JwtAuthentication_JsonResult
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
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


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

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey")))
                };
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp 99 API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                                }
                            },
                            new string[] {}
                }});

            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
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