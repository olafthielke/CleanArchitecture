using Amazon;
using Amazon.SimpleEmail;
using Notification.Email.AWS.Interfaces;

namespace Notification.Email.AWS.Services
{
    public class AmazonSimpleEmailServiceClientFactory : IAmazonSimpleEmailServiceClientFactory
    {
        public IAmazonSimpleEmailService Create(RegionEndpoint region)
        {
            return new AmazonSimpleEmailServiceClient(region);
        }
    }
}
