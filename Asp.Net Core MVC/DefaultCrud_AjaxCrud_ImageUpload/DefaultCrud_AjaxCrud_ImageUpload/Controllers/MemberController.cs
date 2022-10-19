using AutoMapper;
using DefaultCrud_AjaxCrud_ImageUpload.Data;
using DefaultCrud_AjaxCrud_ImageUpload.Entities;
using DefaultCrud_AjaxCrud_ImageUpload.Models.UserViewModels;
using DefaultCrud_AjaxCrud_ImageUpload.PasswordSaltHash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DefaultCrud_AjaxCrud_ImageUpload.Controllers
{
    public class MemberController : Controller
    {

        private readonly AppDbContext context;
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IMapper mapper;

        public MemberController(AppDbContext context, IMapper mapper, IPasswordGenerator passwordGenerator)
        {
            this.context = context;
            this.mapper = mapper;
            this.passwordGenerator = passwordGenerator;
        }

        public IActionResult Index()
        {
            return View();
        }

        // List

        public IActionResult MemberListPartial()
        {
            var users = context.Users.ToList();
            var result = mapper.Map<List<UserListViewModel>>(users);
            return PartialView("_MemberListPartial", result);
        }

        // Add
        public IActionResult MemberUserCreatePartial()
        {
            return PartialView("_MemberUserCreatePartial", new UserCreateViewModel());
        }

        [HttpPost]
        public IActionResult AddNewUser(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return PartialView("_MemberUserCreatePartial", model);

                }
                var user = mapper.Map<User>(model);
                user.Password = passwordGenerator.PasswordSaltAndHash(model.Password);
                context.Users.Add(user);
                context.SaveChanges();
                return PartialView("_MemberUserCreatePartial", new UserCreateViewModel { Done = "User is added successfully." });

            }
            return PartialView("_MemberUserCreatePartial", model);
        }

        // Update
        public IActionResult MemberUserEditPartial(Guid id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);
            UserEditViewModel model = mapper.Map<UserEditViewModel>(user);
            return PartialView("_MemberUserEditPartial", model);
        }

        [HttpPost]
        public IActionResult EditUser(UserEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id !=model.Id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return PartialView("_MemberUserEditPartial", model);

                }
                User user = context.Users.SingleOrDefault(x => x.Id == model.Id);
                mapper.Map(model, user);
                context.SaveChanges();
                return PartialView("_MemberUserEditPartial", new UserEditViewModel { Done = "User is updated successfully." });

            }
            return PartialView("_MemberUserEditPartial", model);
        }

        // Delete

        [HttpGet]
        public IActionResult DeleteUser(Guid id)
        {
            User user = context.Users.SingleOrDefault(u => u.Id == id);

            if(user is not null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }

            return MemberListPartial();

        }
    }
}
