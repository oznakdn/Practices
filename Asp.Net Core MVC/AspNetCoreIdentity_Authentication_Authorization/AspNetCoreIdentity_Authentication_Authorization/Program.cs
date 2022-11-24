using AspNetCoreIdentity_Authentication_Authorization.CustomValidations;
using AspNetCoreIdentity_Authentication_Authorization.Data;
using AspNetCoreIdentity_Authentication_Authorization.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//DbContext conf.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("AppConnection")));

// Identity conf.
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = true; // Email benzersiz olmali
    options.User.AllowedUserNameCharacters = "abcdefghijklmnoqprstuvwxyzABCDEFGHIJKLMNOQPRSTUVWXYZ0123456789-._@+"; // Kullanicinin UserName'e girebilecegi karakterler
    options.Password.RequiredLength = 6; // Sifre en az 6 karakter olmali
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false; // 0-9 ardisik sayi girilemez
    options.Password.RequireUppercase = false; // Buyuk harf kullanimi zorunlu degil
    options.Password.RequireLowercase = false; // Kucuk harf kullanimi zorunlu degil
    //options.Lockout.MaxFailedAccessAttempts = 3; // 3 kez ust uste girildiginde 
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //5 dk sistem girisini engeller

})
.AddPasswordValidator<CustomPasswordValidator>()
.AddUserValidator<CustomUserValidator>()
.AddErrorDescriber<CustomIdentityErrorDescriber>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


// Cookie conf.
//CookieAuthenticationDefaults.AuthenticationScheme

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.Cookie.Name = "MyProject";
        options.Cookie.Expiration = TimeSpan.FromDays(60);
        options.Cookie.HttpOnly = false;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.LoginPath = new PathString("/Home/Login");
        options.ExpireTimeSpan = TimeSpan.FromDays(60);
        options.SlidingExpiration = true;
    });

//CookieBuilder cookieBuilder = new()
//{
//    Name = "MyProject", // Browser'da cookie'nin gosterildigi deger
//    HttpOnly = false, // Sadece Http isteklerinde gosterilsin
//    Expiration = TimeSpan.FromDays(60), // Clien 60 gun boyunca username ve password girmeden siteye girebilir
//    SameSite = SameSiteMode.Lax, // Cookie kaytdedildikten sonra herhangi bir site uzerinden cookie degerine ulasilabilir.
//    //SameSite = SameSiteMode.Strict ====>  Cookie kaytdedildikten sonra sadece ilgili site uzerinden cookie degerine ulasilabilir. (Siteler arasi istek hirsizligini engeller.)
//    SecurePolicy = CookieSecurePolicy.SameAsRequest,  //Browser sitenin Http yada Https istek sekline gore kullanici cookie'sini gonderir
//    //SecurePolicy = CookieSecurePolicy.Always  ====> Browser sadece Https uzerinden istek gelirse kullanici cookie'sini gonderir
//    //SecurePolicy = CookieSecurePolicy.None // Browser  Http yada Https uzerinden istek gelirse kullanici cookie'sini gonderir

//};

//builder.Services.ConfigureApplicationCookie(opt =>
//{
//    opt.Cookie = cookieBuilder;
//    opt.LoginPath = new PathString("/Home/Login");
//    //opt.LogoutPath = new PathString("/Home/Logout");
//    //opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
//    opt.SlidingExpiration = true; // Cookie suresini Expiration kadar daha uzatir.
//});




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
