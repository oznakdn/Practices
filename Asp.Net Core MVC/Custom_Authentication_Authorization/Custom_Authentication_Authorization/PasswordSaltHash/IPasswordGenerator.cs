namespace Custom_Authentication_Authorization.PasswordSaltHash
{
    public interface IPasswordGenerator
    {
        string PasswordSaltAndHash(string password);
    }
}
