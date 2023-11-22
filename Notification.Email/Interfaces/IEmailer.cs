using System.Net.Mail;
using System.Threading.Tasks;

namespace Notification.Email.Interfaces
{
    public interface IEmailer
    {
        Task Send(MailMessage email);
    }
}
