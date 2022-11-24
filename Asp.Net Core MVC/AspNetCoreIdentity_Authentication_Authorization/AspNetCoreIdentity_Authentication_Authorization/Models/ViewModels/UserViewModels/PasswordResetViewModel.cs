using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Authentication_Authorization.Models.ViewModels.UserViewModels
{
    public class PasswordResetViewModel
    {

        [Required(ErrorMessage = "{0} is required!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
