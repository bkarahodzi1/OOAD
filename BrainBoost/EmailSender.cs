using NuGet.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BrainBoost
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new MailAddress("brainboostmvc@gmail.com");
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("brainboostmvc@gmail.com", "xxvtcusfebuybzqb");

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
