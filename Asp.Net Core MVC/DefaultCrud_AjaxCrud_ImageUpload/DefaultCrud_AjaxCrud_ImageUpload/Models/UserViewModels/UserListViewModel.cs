using System.ComponentModel.DataAnnotations;

namespace DefaultCrud_AjaxCrud_ImageUpload.Models.UserViewModels
{
    public class UserListViewModel
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string CreatedDate { get; set; }
        public bool Locked { get; set; }
        public string Role { get; set; }


    }
}
