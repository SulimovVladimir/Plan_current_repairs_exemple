using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace Plan_current_repairs.Extensions
{
    public class EmailService :IEmailService
    {
        public async Task SendEmailAsync (string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage ();
            emailMessage.From.Add(new MailboxAddress("План текущего ремонта", "us_info@domen.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("Plain") { Text = message };

            using (var client= new SmtpClient())
            {
                await client.ConnectAsync("server", 25, false);
                await client.AuthenticateAsync("us_info", "password");
                await client.SendAsync (emailMessage);
                await client.DisconnectAsync(true);
            }    
        }
    }
}
