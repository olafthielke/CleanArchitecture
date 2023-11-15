using System.Diagnostics;
using System.Net.Mail;
using Notification.Email.Interfaces;

namespace Notification.Email.Services
{
    public class NullEmailer : IEmailer
    {
        public void Send(MailMessage email)
        {
            // Do Nothing

            Debug.WriteLine($"Sending '{email.Subject}' to customer '{email.To[0]}'");
        }
    }
}
