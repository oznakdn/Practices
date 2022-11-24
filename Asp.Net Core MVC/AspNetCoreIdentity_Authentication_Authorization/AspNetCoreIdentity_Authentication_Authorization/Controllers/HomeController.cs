using AspNetCoreIdentity_Authentication_Authorization.Entities;
using AspNetCoreIdentity_Authentication_Authorization.Helpers;
using AspNetCoreIdentity_Authentication_Authorization.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AspNetCoreIdentity_Authentication_Authorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {

            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(loginView.Email); // Email den kullaniciyi buluyoruz.

                if (user != null) // user varsa
                {

                    if (await _userManager.IsLockedOutAsync(user)) // kullanici kilitli ise
                    {
                        ModelState.AddModelError("", "Your account was locked for a while, please try again");
                        return View(loginView);

                    }

                    await _signInManager.SignOutAsync(); // Mecvutta cookie varsa onu siliyoruz.

                    SignInResult result = await _signInManager.PasswordSignInAsync(user, loginView.Password, loginView.RememberMe, false); // Giris yapiyoruz



                    if (result.Succeeded) // Giris basarili ise
                    {
                        await _userManager.ResetAccessFailedCountAsync(user); // basarili giris yapan kullanicinin basarisiz giris denemeleri sifirlanir

                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString()); // girmek istedigi sayfaya yonlendirir
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else // Giris basarisiz ise
                    {

                        await _userManager.AccessFailedAsync(user); // basarisiz giris sayisini 1'er attirir
                        int failLoginCount = await _userManager.GetAccessFailedCountAsync(user); // basarisiz giris sayisi
                        ModelState.AddModelError("", $"{failLoginCount}. fail try!"); // hatali giris sayisi kullaniciya gosterilir

                        if (failLoginCount == 3) // 3 basarisiz giristen sonra kullanici kilitleniyor
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset().AddMinutes(5)); // kullanici 5 dk kilitlenir
                            ModelState.AddModelError("", "Your account locked for 3 fail login, please try again.");
                            return View(loginView);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email or Password is wrong.");
                            return View(loginView);
                        }
                    }
                }
                else // user yoksa
                {
                    ModelState.AddModelError("", "User is no exists");
                }
            }

            return View(loginView);
        }
        #endregion

        #region ForgetPassword

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordReset)
        {
            AppUser user = await _userManager.FindByEmailAsync(passwordReset.Email);

            if(user is not null)
            {
                string passwordResetToken =await _userManager.GeneratePasswordResetTokenAsync(user);
                string passwordResetLink = Url.Action("ResetPasswordConfirm","Home",new
                {
                    userId = user.Id,
                    token = passwordResetToken,
                },HttpContext.Request.Scheme); 

                PasswordResetHelper.PasswordResetSendEmail(passwordResetLink, passwordReset.Email);

                ViewBag.status = "Successfully";

            }
            else
            {
                ModelState.AddModelError("", "Email address is no exists!");
            }


            return View(passwordReset);
        }


        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(PasswordResetConfirmViewModel passwordReset)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if(user is not null) 
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, passwordReset.Password);
                
                if(result.Succeeded) 
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    TempData["passwordResetInfo"] = "Your password was refreshed successful, you can login with your new password.";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return View();
        }




        #endregion


    }
}