using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace EA_Ecommerce.PL.utils
{
    public class EmailSetting : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSetting(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var password = emailSettings["Password"];
            var emailAdmin = emailSettings["email"];

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailAdmin, password)
            };

            return client.SendMailAsync(new MailMessage(from: emailAdmin!, to: email, subject, htmlMessage)
            {
                IsBodyHtml = true
            });
        }
    }
}
