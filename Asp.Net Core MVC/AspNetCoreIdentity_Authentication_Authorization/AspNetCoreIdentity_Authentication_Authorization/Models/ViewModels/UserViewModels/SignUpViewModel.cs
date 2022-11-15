using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace AspNetCoreIdentity_Authentication_Authorization.Models.ViewModels.UserViewModels
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage ="{0} is required!")]
        [DataType(DataType.EmailAddress,ErrorMessage ="{0} must be Email format!")]
        public string Email { get; set; }


        [Required(ErrorMessage ="{0} is required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.Password)]
        [MaxLength(12,ErrorMessage ="{0} must be max 12 characters")]
        [MinLength(6,ErrorMessage ="{0} must be min 6 characters!")]
        public string Password { get; set; }



        [DataType(DataType.PhoneNumber,ErrorMessage ="{0} must be phone number format!")]
        public string PhoneNumber { get; set; }
    }
}
