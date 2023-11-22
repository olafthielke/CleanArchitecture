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
            // Do Nothing

            Debug.WriteLine($"Sending '{email.Subject}' to customer '{email.To[0]}'");
        }
    }
}
