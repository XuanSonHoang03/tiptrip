using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using TipTrip.Common.Models;
using TipTrip.Application.Implements.Interfaces;
using TipTrip.Services;

namespace TipTrip.Application.Implements.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value; // Lấy giá trị cấu hình
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, "oryb hbnh fdpa koct"),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.Name),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
