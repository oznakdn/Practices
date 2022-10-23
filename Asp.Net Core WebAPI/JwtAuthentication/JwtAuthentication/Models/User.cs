namespace JwtAuthentication.Models
{
    public class User
    {
        public User()
        {
            Role = "Standard";
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }



    }
}
