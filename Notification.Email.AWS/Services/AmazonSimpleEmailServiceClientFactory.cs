using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Notification.Email.AWS.Interfaces;

namespace Notification.Email.AWS.Services
{
    public class AmazonSimpleEmailServiceClientFactory(AmazonConfiguration config)
        : IAmazonSimpleEmailServiceClientFactory
    {
        private AmazonConfiguration Config { get; } = config;

        public IAmazonSimpleEmailService Create()
        {
            var credentials = new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);

            var region = RegionEndpoint.GetBySystemName(Config.Region);

            return new AmazonSimpleEmailServiceClient(credentials, region);
        }
    }
}
