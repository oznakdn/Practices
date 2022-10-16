namespace DefaultCrud_AjaxCrud_ImageUpload.PasswordSaltHash
{
    public interface IPasswordGenerator
    {
        string PasswordSaltAndHash(string password);
    }
}
