using AspNetCoreIdentity_Authentication_Authorization.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity_Authentication_Authorization.CustomValidations
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new();


            if(password.ToLower().Contains(user.UserName.ToLower()))
            {
                if(!user.Email.Contains(user.UserName))
                {
                    errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "Sifre alani kullanici adi iceremez!" });
                }
            }

            if(password.ToLower().Contains("123456789"))
            {
                errors.Add(new IdentityError() { Code = "PasswordContains123456789", Description = "Sifre alani ardisik sayi iceremez!" });

            }

            if(password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainsEmail", Description = "Sifre alani email adresiniz iceremez!" });

            }

            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }


        }
    }
}
