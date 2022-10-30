using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication_JsonResult.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(30, ErrorMessage = "{0} must be equal or less than 30 characters!")]
        [MinLength(5, ErrorMessage = "{0} must be equal or greater than 5 characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(6, ErrorMessage = "{0} must be equal or greater than 6 characters!")]
        [MaxLength(16, ErrorMessage = "{0} must be equal or less than 30 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
