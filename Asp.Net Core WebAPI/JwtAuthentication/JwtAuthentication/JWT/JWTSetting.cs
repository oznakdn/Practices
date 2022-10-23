namespace JwtAuthentication.JWT
{
    public class JWTSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
    }
}
