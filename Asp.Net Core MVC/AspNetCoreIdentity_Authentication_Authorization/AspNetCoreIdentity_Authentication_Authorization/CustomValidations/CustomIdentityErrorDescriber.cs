using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity_Authentication_Authorization.CustomValidations
{
    public class CustomIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                 Code = "IdentityUserName",
                 Description = $"Bu {userName} gecersizdir."
            };

        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuclicateEmail",
                Description = $"Bu {email} kullanilmaktadir."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"Sifre en az {length} karakterden olusmalidir."
            };
        }
    }
}
