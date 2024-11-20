using Services.Contracts;
using System.Net;
using System.Net.Mail;

namespace Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer = "smtp.gmail.com";  
        private readonly int _smtpPort = 587;  
        private readonly string _smtpUser = "secilseleciquizhub@gmail.com";  
        private readonly string _smtpPass = "usqd lgfa jube hced";  

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
