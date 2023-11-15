using Notification.Email.Interfaces;

namespace Notification.Email.Services
{
    public class HardcodedEmailConfiguration : IEmailConfiguration
    {
        public string FromAddress => "olaf@codecoach.co.nz";
    }
}
