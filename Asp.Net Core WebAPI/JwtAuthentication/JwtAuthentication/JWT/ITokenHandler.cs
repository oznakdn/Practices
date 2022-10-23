using JwtAuthentication.Models;

namespace JwtAuthentication.JWT
{
    public interface ITokenHandler
    {
        TokenResponse CreateAccessToken(User user);
        string CreateRefreshToken();
        //ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
