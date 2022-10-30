using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication_JsonResult.Controllers
{
    //[Authorize(Roles ="Admin,Manager")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
