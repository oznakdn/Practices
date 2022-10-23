using JwtAuthentication.Data;
using JwtAuthentication.Dtos.UserDtos;
using JwtAuthentication.JWT;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext context;
        private readonly ITokenHandler handle;

        public UsersController(AppDbContext context, ITokenHandler handle)
        {
            this.context = context;
            this.handle = handle;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUser)
        {
            int isSaved = 0;
            var existUser = context.Users.Any(u => u.Username == registerUser.Username);
            if (existUser)
            {
                return BadRequest("User is already exists!");
            }

            if (ModelState.IsValid)
            {
                User newUser = new()
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    Username = registerUser.Username,
                    Password = registerUser.Password
                };
                await context.Users.AddAsync(newUser);
                isSaved = await context.SaveChangesAsync();
                if (isSaved == 0)
                    return BadRequest();
                return Ok(newUser);
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            var existUser = await context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username && u.Password == loginUser.Password);

           if(existUser!=null)
            {
                var token = handle.CreateAccessToken(existUser);

                existUser.RefreshToken = token.RefreshToken;
                existUser.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(5); // refresh token gecerlilik suresi
                await context.SaveChangesAsync();
                return Ok(new
                {
                    token.RefreshToken,
                    token.AccessToken
                });
            }
            return Unauthorized();
        }


        [HttpPost("RefreshLogin")]
        public async Task<IActionResult> RefreshLogin(string refreshToken)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user.RefreshTokenExpireTime > DateTime.UtcNow)
            {
                var accessToken = handle.CreateAccessToken(user);
                user.RefreshToken = accessToken.RefreshToken;
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(5);
                await context.SaveChangesAsync();
                return Ok(new
                {
                    accessToken = accessToken.AccessToken,
                    refreshToken = accessToken.RefreshToken
                });

            }

            return NotFound("Not found!");

        }
    }
}
