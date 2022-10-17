using System.ComponentModel.DataAnnotations;

namespace DefaultCrud_AjaxCrud_ImageUpload.Models.UserViewModels
{
    public class UserCreateViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }


        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(30, ErrorMessage = "{0} must be equal or less than 30 characters!")]
        [MinLength(5, ErrorMessage = "{0} must be equal or greater than 5 characters!")]
        public string Username { get; set; }


        [Required(ErrorMessage = "{0} is required!")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        public bool Locked { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(6, ErrorMessage = "{0} must be equal or greater than 6 characters!")]
        [MaxLength(16, ErrorMessage = "{0} must be equal or less than 30 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(6, ErrorMessage = "{0} must be equal or greater than 6 characters!")]
        [MaxLength(16, ErrorMessage = "{0} must be equal or less than 30 characters!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "{0} must be the same with the password")]
        public string PasswordConfirm { get; set; }
    }
}
