using System.Threading.Tasks;

namespace BrainBoost
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
