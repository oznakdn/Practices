using JwtAuthentication_JsonResult.Helpers;
using JwtAuthentication_JsonResult.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication_JsonResult.Controllers.APIs
{
    [Authorize(Roles ="Admin",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestoController : ControllerBase
    {
        private readonly ITokenHelper token;

        public TestoController(ITokenHelper token)
        {
            this.token = token;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok(new { Name = "John", Surname = "Doe" });
        }

        [HttpPost]
        public IActionResult PostData([FromBody] PostDataApiModel model)
        {
            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var accessToken = token.CreateToken(model.Username, model.Password);
            if (accessToken == null) return NotFound("User not found");
            return Ok(accessToken);
        }
    }
}
