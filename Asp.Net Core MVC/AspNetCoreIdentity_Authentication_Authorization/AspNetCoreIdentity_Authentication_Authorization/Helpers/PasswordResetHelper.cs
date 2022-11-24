using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentity_Authentication_Authorization.Helpers
{
    public static class PasswordResetHelper
    {
        public static void PasswordResetSendEmail(string link, string emailAddress)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.mywebsite.com");

            mail.From = new MailAddress("info@mywebsite.com");
            mail.To.Add(emailAddress);
            mail.Subject = $"www.mywebsite.com::Password Reset";
            mail.Body = "<h2>For changing your password, please click the link...</h2><hr/>";
            mail.Body += $"<a href='{link}'>Password Reset Link</a>";
            mail.IsBodyHtml = true;


            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("info@mywebsite.com", "mypassword123");
            smtpClient.Send(mail);
        }
    }
}
