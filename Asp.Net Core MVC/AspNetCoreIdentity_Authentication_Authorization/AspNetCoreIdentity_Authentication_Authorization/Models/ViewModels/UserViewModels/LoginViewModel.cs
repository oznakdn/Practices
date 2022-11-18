using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Authentication_Authorization.Models.ViewModels.UserViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "{0} is required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.Password)]
        [MaxLength(12, ErrorMessage = "{0} must be max 12 characters")]
        [MinLength(6, ErrorMessage = "{0} must be min 6 characters!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
