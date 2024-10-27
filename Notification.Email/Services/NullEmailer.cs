using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;
using Notification.Email.Interfaces;

namespace Notification.Email.Services
{
    public class NullEmailer : IEmailer
    {
        public async Task Send(MailMessage email)
        {
            // Only log to console

            Debug.WriteLine($"\nSending '{email.Subject}' to email '{email.To[0]}'\n");
        }
    }
}
