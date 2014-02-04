using AddMFAAuthSample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace AddMFAAuthSample.App_Start
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(UserStore<ApplicationUser> userStore)
            : base(userStore)
        {

        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            manager.PasswordValidator = new MinimumLengthValidator(10);

            manager.EmailService = new EmailService();

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>()
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            }); 

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            } 

            return manager;
        } 
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage email = new MailMessage("XXX@hotmail.com", message.Destination);

            email.Subject = message.Subject;

            email.Body = message.Body;

            email.IsBodyHtml = true;

            var mailClient = new SmtpClient("smtp.live.com", 587) { Credentials = new NetworkCredential("XXX@hotmail.com", "password"), EnableSsl = true };

            return mailClient.SendMailAsync(email);
        }
    } 
}