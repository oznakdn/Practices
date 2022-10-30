using JwtAuthentication_JsonResult.Data;
using JwtAuthentication_JsonResult.PasswordSaltHash;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication_JsonResult.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly IPasswordGenerator generator;

        public TokenHelper(AppDbContext context, IConfiguration configuration, IPasswordGenerator generator)
        {
            this.context = context;
            this.configuration = configuration;
            this.generator = generator;
        }

        public string CreateToken(string username, string password)
        {
            var pass = generator.PasswordSaltAndHash(password);
            var user = context.Users.SingleOrDefault(x => x.Username == username && x.Password == pass);

            if (user != null)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:SecretKey")));

                SigningCredentials _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                List<Claim> _claims = new()
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };


                JwtSecurityToken token = new JwtSecurityToken(signingCredentials: _signingCredentials, claims: _claims, expires:DateTime.Now.AddMinutes(10));
                var accessToken = new JwtSecurityTokenHandler();
                return accessToken.WriteToken(token);

            }
            return null;
        }
    }
}
