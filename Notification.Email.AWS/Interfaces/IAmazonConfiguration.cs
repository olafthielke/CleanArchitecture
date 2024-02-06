using Amazon;

namespace Notification.Email.AWS.Interfaces
{
    public interface IAmazonConfiguration
    {
        RegionEndpoint Region { get; }
    }
}
