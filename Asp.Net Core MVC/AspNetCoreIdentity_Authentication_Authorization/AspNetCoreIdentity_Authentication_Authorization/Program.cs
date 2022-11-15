using AspNetCoreIdentity_Authentication_Authorization.CustomValidations;
using AspNetCoreIdentity_Authentication_Authorization.Data;
using AspNetCoreIdentity_Authentication_Authorization.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DbContext conf.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("AppConnection")));

// Identity conf.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true; // Email benzersiz olmali
    options.User.AllowedUserNameCharacters = "abcdefghijklmnoqprstuvwxyzABCDEFGHIJKLMNOQPRSTUVWXYZ0123456789-._@+"; // Kullanicinin girebilecegi karakterler
    options.Password.RequiredLength = 6; // Sifre en az 6 karakter olmali
    options.Password.RequiredUniqueChars = 0; 
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false; // 0-9 ardisik sayi girilemez
    options.Password.RequireUppercase = false; // Buyuk harf kullanimi zorunlu degil
    options.Password.RequireLowercase = false; // Kucuk harf kullanimi zorunlu degil
    options.Lockout.MaxFailedAccessAttempts = 5; // 5 kez ust uste girildiginde 5 dk sistem girisini engeller
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

})
.AddPasswordValidator<CustomPasswordValidator>()
.AddEntityFrameworkStores<AppDbContext>();





var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
