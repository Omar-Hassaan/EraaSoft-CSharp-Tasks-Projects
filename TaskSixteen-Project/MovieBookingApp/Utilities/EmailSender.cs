using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace MovieBookingApp.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("omarhassaann2@gmail.com", "iokz crbo kroy qgrt")
            };
            var mail = new MailMessage(from: "omarhassaann2@gmail.com", to: email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };
            return client.SendMailAsync(mail);
        }
    }
}
