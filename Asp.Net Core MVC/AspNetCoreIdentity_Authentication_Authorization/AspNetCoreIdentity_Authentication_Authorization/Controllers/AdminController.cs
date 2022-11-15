using AspNetCoreIdentity_Authentication_Authorization.Entities;
using AspNetCoreIdentity_Authentication_Authorization.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity_Authentication_Authorization.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var usersViewModel = users.Select(user => new GetUserViewModel
            {
                 Id = user.Id,
                 Username = user.UserName,
                 Email = user.Email,
                 PhoneNumber = user.PhoneNumber
            }).ToList();
            return View(usersViewModel);
        }
    }
}
