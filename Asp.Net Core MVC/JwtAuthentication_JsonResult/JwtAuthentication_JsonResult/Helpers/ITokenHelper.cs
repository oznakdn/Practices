namespace JwtAuthentication_JsonResult.Helpers
{
    public interface ITokenHelper
    {
        string CreateToken(string username, string password);
    }
}
