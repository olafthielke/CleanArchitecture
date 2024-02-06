using Amazon;
using Microsoft.Extensions.Options;
using Notification.Email.AWS.Interfaces;
using Notification.Email.AWS.Models;

namespace Notification.Email.AWS.Services
{
    public class AppSettingsAmazonConfiguration(IOptions<AwsConfig> options) : IAmazonConfiguration
    {
        public RegionEndpoint Region => GetRegion();
        private IOptions<AwsConfig> Options { get; } = options;

        public RegionEndpoint GetRegion()
        {
            var regionName = Options.Value.Region;
            return regionName == null
                ? RegionEndpoint.USEast1 // Config Default -> Great Idea!!
                : RegionEndpoint.GetBySystemName(regionName);
        }
    }
}