using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Options;
using Notification.Email.AWS.Interfaces;

namespace Notification.Email.AWS.Services
{
    public class AmazonSimpleEmailServiceClientFactory(IOptions<AmazonConfiguration> config)
        : IAmazonSimpleEmailServiceClientFactory
    {
        private IAmazonConfiguration Config { get; } = config.Value;

        public IAmazonSimpleEmailService Create()
        {
            var region = RegionEndpoint.GetBySystemName(Config.Region);
            var credentials = new BasicAWSCredentials(Config.AccessKey, Config.SecretKey);

            return new AmazonSimpleEmailServiceClient(credentials, region);
        }
    }
}
