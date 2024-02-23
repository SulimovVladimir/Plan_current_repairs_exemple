using System.Threading.Tasks;

namespace Plan_current_repairs.Extensions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
