using System.Net.Mail;

namespace Notification.Email.Interfaces
{
    public interface IEmailer
    {
        void Send(MailMessage email);
    }
}
