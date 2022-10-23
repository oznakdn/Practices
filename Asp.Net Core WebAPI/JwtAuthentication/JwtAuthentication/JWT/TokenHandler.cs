using JwtAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthentication.JWT
{
    public class TokenHandler : ITokenHandler
    {
        //private readonly JWTSetting setting;
        //public TokenHandler(IOptions<JWTSetting> options)
        //{
        //    setting = options.Value;
        //}

        private readonly IConfiguration configuration;

        public TokenHandler(IConfiguration configuration)
        {
           this.configuration = configuration;
        }

        public TokenResponse CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTSetting:SecurityKey")));
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = configuration.GetValue<string>("JWTSetting:Issuer"),
                Audience = configuration.GetValue<string>("JWTSetting:Audience"),
                Subject = new ClaimsIdentity
                 (
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    }
                 ),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Expires = DateTime.Now.AddMinutes(3) // access token gecerlilik suresi
            };

            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(createdToken);
            var refreshToken = CreateRefreshToken();

            return new TokenResponse
            {
                 AccessToken = accessToken,
                 RefreshToken = refreshToken,
            };

            


        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            string refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken;
        }

        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        //{
        //    var Key = Encoding.UTF8.GetBytes(configuration["JWTSetting:SecurityKey"]);

        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Key),
        //        ClockSkew = TimeSpan.Zero
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        //    JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }


        //    return principal;
        //}
    }
}
