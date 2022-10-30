using AutoMapper;
using JwtAuthentication_JsonResult.Data;
using JwtAuthentication_JsonResult.Entities;
using JwtAuthentication_JsonResult.Models.UserViewModels;
using JwtAuthentication_JsonResult.PasswordSaltHash;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication_JsonResult.Controllers
{
    [Authorize(Roles ="Admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly AppDbContext context;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IMapper mapper;

        public UserController(AppDbContext context, IMapper mapper, IPasswordGenerator passwordGenerator)
        {
            this.context = context;
            this.mapper = mapper;
            this.passwordGenerator = passwordGenerator;
        }


        #region Read
        public IActionResult Index()
        {
            var users = context.Users.ToList();
            var usersModel = mapper.Map<List<UserListViewModel>>(users);

            return View(usersModel.ToList());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.Any(u => u.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }

                User user = mapper.Map<User>(model);
                user.Password = passwordGenerator.PasswordSaltAndHash(model.Password);
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        #endregion

        #region Update
        public IActionResult Edit(Guid id)
        {
            User user = context.Users.Find(id);
            UserEditViewModel viewModel = mapper.Map<UserEditViewModel>(user);
            return View(viewModel);

        }

        [HttpPost]
        public IActionResult Edit(UserEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (context.Users.Any(u => u.Username.ToLower() == model.Username.ToLower() && u.Id != model.Id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }
                User user = context.Users.SingleOrDefault(x => x.Id == model.Id);
                mapper.Map(model, user);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        #endregion

        #region Delete

        [HttpGet]
        public IActionResult Delete(Guid id)
        {

            var user = context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion




    }
}
