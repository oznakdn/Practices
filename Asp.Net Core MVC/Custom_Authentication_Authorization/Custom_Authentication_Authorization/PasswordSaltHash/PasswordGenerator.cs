using NETCore.Encrypt.Extensions;

namespace Custom_Authentication_Authorization.PasswordSaltHash
{
    public class PasswordGenerator:IPasswordGenerator
    {
        private readonly IConfiguration configuration;

        public PasswordGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string PasswordSaltAndHash(string password)
        {
            string md5Salt = configuration.GetValue<string>("SaltSetting:MD5Salt");
            string salted = $"{password}{md5Salt}";
            string hashed = salted.MD5();
            return hashed;
        }
    }
}
