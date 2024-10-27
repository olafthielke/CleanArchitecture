using System.Diagnostics;
using System.Threading.Tasks;
using Notification.SMS.Interfaces;
using Notification.SMS.Models;

namespace Notification.SMS.Services
{
    public class NullSmsSender : ISmsSender
    {
        public async Task Send(SmsMessage message)
        {
            // Only log to console

            Debug.WriteLine($"\nSending '{message.Body}' to mobile '{message.To}'\n");
        }
    }
}
