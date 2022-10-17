using DefaultCrud_AjaxCrud_ImageUpload.Data;
using DefaultCrud_AjaxCrud_ImageUpload.Entities;
using DefaultCrud_AjaxCrud_ImageUpload.Models;
using DefaultCrud_AjaxCrud_ImageUpload.PasswordSaltHash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DefaultCrud_AjaxCrud_ImageUpload.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly IPasswordGenerator passwordGenerator;

        public AccountController(AppDbContext context, IConfiguration configuration, IPasswordGenerator passwordGenerator)
        {
            this.context = context;
            this.configuration = configuration;
            this.passwordGenerator = passwordGenerator;
        }


        #region Login

        // GET
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await context.Users.Where(u => u.Password == passwordGenerator.PasswordSaltAndHash(model.Password) && u.Username == model.Username).SingleOrDefaultAsync();

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(user.Username), "User is locked!");
                        return View(model);
                    }

                    // Cookie eklemek icin
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim("FullName", $"{user.Firstname} {user.Lastname}" ?? String.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));


                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Username or Password is wrong!");
                }
            }
            ViewData["error"] = "Username or Password is wrong!";
            return View(model);
        }



        #endregion

        #region Register

        // GET

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            int isSaved = 0;
            if (ModelState.IsValid)
            {
                // Kullanici var mi? Kontrol eder.
                if (context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }

                // Kullanici mevcut degil ise yeni kullanici ekler.
                context.Users.Add(new User
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    EmailAddress = model.Email,
                    Username = model.Username,
                    Password = passwordGenerator.PasswordSaltAndHash(model.Password)
                }); ;
                isSaved = context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }

            // Kayit basarisiz olursa
            if (isSaved == 0)
            {
                ModelState.AddModelError("Hata", "User could not added!");
            }
            return View(model);

        }

        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region Profile

        public IActionResult Profile()
        {

            // User Id bulmak
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // User Id den kullaniciyi bulmak
            User user = context.Users.Single(u => u.Id == userId);

            // User Firstname ve Lastname bilgilerini ilgili Input'larda gostermek
            ViewData["profileImage"] = user.ImageFileName;
            ViewData["Firstname"] = user.Firstname;
            ViewData["Lastname"] = user.Lastname;

            return View();
        }



        #endregion

        #region Change Profile Informations

        #region Change Name-Surname
        [HttpPost]
        public IActionResult ProfileChangeNameSurname([Required][StringLength(30)] string? firstname, [Required][StringLength(30)] string? lastname)
        {
            if (ModelState.IsValid)
            {
                // User Id bulmak
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // User Id den kullaniciyi bulmak
                User user = context.Users.SingleOrDefault(u => u.Id == userId);


                // User Firstname ve Lastname guncellemek ve kaydetmek
                ViewData["profileImage"] = user.ImageFileName;
                user.Firstname = firstname;
                user.Lastname = lastname;
                context.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }
        #endregion

        #region Change Password

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                // User Id bulmak
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // User Id den kullaniciyi bulmak
                User user = context.Users.SingleOrDefault(u => u.Id == userId);

                // User password guncellemek ve kaydetmek
                user.Password = passwordGenerator.PasswordSaltAndHash(password);
                context.SaveChanges();

                ViewData["profileImage"] = user.ImageFileName;
                ViewData["result"] = "PasswordChanged";
                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }

        #endregion

        #region Change EmailAddress

        [HttpPost]
        public IActionResult ProfileChangeEmailAddress([Required][DataType(DataType.EmailAddress)] string? emailAddress)
        {
            if (ModelState.IsValid)
            {
                // User Id bulmak
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // User Id den kullaniciyi bulmak
                User user = context.Users.SingleOrDefault(u => u.Id == userId);

                // User password guncellemek ve kaydetmek
                user.EmailAddress = emailAddress;
                context.SaveChanges();

                ViewData["profileImage"] = user.ImageFileName;
                ViewData["emailAddress"] = user.EmailAddress;
                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }
        #endregion

        #region Change Image

        [HttpPost]
        public IActionResult ProfileChangeImage([Required] IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = context.Users.Single(u => u.Id == userId);

                // Resim kaydetme stili
                string fileName = $"p-{userId}.jpg";

                // Resim kaydetme
                using Stream stream = new FileStream($"wwwroot/uploads/{fileName}", FileMode.OpenOrCreate);
                imageFile.CopyTo(stream);
                stream.Close();

                // Veritabanina eklemek
                user.ImageFileName = fileName;
                context.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }

        #endregion

        #endregion














    }
}
