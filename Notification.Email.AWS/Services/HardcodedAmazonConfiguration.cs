using Amazon;
using Notification.Email.AWS.Interfaces;

namespace Notification.Email.AWS.Services
{
    // Useful for DEV environment.
    public class HardcodedAmazonConfiguration : IAmazonConfiguration
    {
        public RegionEndpoint Region => RegionEndpoint.APSoutheast2;
    }
}
