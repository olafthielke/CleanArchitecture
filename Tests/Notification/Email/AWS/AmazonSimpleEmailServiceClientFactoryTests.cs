using Amazon;
using Xunit;
using FluentAssertions;
using Notification.Email.AWS.Services;

namespace Tests.Notification.Email.AWS
{
    public class AmazonSimpleEmailServiceClientFactoryTests
    {
        [Fact]
        public void When_Call_Create_Then_Creates_Client()
        {
            var config = new AmazonConfiguration { Region = "us-east-1", AccessKey = "ABC", SecretKey = "123" };
            var factory = new AmazonSimpleEmailServiceClientFactory(config);

            var client = factory.Create();

            client.Should().NotBeNull();
            client.Config.RegionEndpoint.Should().Be(RegionEndpoint.USEast1);
        }
    }
}
