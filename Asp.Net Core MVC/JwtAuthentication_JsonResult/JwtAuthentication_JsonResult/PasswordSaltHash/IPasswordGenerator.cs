namespace JwtAuthentication_JsonResult.PasswordSaltHash
{
    public interface IPasswordGenerator
    {
        string PasswordSaltAndHash(string password);
    }
}
