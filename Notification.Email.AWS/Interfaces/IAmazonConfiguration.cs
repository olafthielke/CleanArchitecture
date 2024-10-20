using Amazon;

namespace Notification.Email.AWS.Interfaces
{
    public interface IAmazonConfiguration
    {
        string Region { get; }
        string AccessKey { get; }
        string SecretKey { get; }
    }
}
