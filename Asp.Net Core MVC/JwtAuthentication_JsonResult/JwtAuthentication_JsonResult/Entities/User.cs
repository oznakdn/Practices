using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_JsonResult.Entities
{
    public class User
    {
        public User()
        {
            this.Locked = false;
            this.CreatedDate = DateTime.Now;
            this.Role = "User";
            this.ImageFileName = "Noimage.jpg";
        }

        public Guid Id { get; set; }

        [StringLength(30)]
        public string? Firstname { get; set; }

        [StringLength(30)]
        public string? Lastname { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool Locked { get; set; }
        public DateTime CreatedDate { get; set; }

        [StringLength(255)]
        public string? ImageFileName { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }


    }
}
