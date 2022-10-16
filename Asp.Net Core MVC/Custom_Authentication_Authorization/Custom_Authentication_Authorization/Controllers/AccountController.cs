using Custom_Authentication_Authorization.Data;
using Custom_Authentication_Authorization.Entities;
using Custom_Authentication_Authorization.Models;
using Custom_Authentication_Authorization.PasswordSaltHash;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Custom_Authentication_Authorization.Controllers
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


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

                    // Cookie icin
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

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            int isSaved = 0;
            //string md5Salt = configuration.GetValue<string>("SaltSetting:MD5Salt");
            //string saltedPassword = $"{model.Password}{md5Salt}";
            //string hashedPassword = saltedPassword.MD5();

            if (ModelState.IsValid)
            {
                if (context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }

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

            if (isSaved == 0)
            {
                ModelState.AddModelError("Hata", "User could not added!");
            }
            return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Profile()
        {
            Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = context.Users.Single(u => u.Id == userId);

            ViewData["Firstname"] = user.Firstname;
            ViewData["Lastname"] = user.Lastname;

            return View();
        }

        [HttpPost]
        public IActionResult ProfileChangeNameSurname([Required][StringLength(30)] string? firstname, [Required][StringLength(30)] string? lastname)
        {
            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = context.Users.Single(u => u.Id == userId);

                user.Firstname = firstname;
                user.Lastname = lastname;
                context.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if(ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = context.Users.Single(u => u.Id == userId);
                user.Password = passwordGenerator.PasswordSaltAndHash(password);
                context.SaveChanges();
                ViewData["result"] = "PasswordChanged";
                return RedirectToAction(nameof(Profile));
            }

            return View("Profile");
        }


    }
}
