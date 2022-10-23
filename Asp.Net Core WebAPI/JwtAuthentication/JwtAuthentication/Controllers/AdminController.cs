using JwtAuthentication.Data;
using JwtAuthentication.Dtos.UserDtos;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext context;

        public AdminController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await context.Users.Select(u => new GetUsersDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role
            }).ToListAsync();

            if(users.Count == 0)
            {
                return BadRequest("There is no user!");
            }

            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> EditUserRole([FromBody] EditUserRoleDto editUser)
        {
            User existUser = await context.Users.FirstOrDefaultAsync(u => u.Id == editUser.UserId);
            if (existUser == null)
                return BadRequest("User not found!");

            existUser.Role = editUser.Role;
            context.Users.Update(existUser);
            await context.SaveChangesAsync();
            return Ok(editUser);
        }
    }
}
