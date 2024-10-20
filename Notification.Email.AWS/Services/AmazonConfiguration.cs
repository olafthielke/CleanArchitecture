using Notification.Email.AWS.Interfaces;

namespace Notification.Email.AWS.Services
{
    public class AmazonConfiguration : IAmazonConfiguration
    {
        public required string Region { get; set; }
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
    }
}
