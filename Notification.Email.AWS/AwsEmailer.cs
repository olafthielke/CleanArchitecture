using System.Net.Mail;
using Microsoft.Extensions.Options;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BusinessLogic.Exceptions;
using Notification.Email.AWS.Interfaces;
using Notification.Email.Interfaces;
using Notification.Email.AWS.Models;

namespace Notification.Email.AWS
{
    public class AwsEmailer(
        IAmazonSimpleEmailServiceClientFactory clientFactory,
        IOptions<AwsConfig> awsConfigOptions)
        : IEmailer
    { 
        private IAmazonSimpleEmailServiceClientFactory ClientFactory { get; } = clientFactory;
        private IOptions<AwsConfig> AwsConfigOptions { get; } = awsConfigOptions;

        public async Task Send(MailMessage email)
        {
            // IOptions Configured AWS Region
            var region = GetConfiguredRegion();
            using var awsClient = ClientFactory.Create(region);
            var request = BuildSendEmailRequest(email);
            await SendAwsEmail(awsClient, request);
        }

        private RegionEndpoint GetConfiguredRegion()
        {
            var regionName = AwsConfigOptions.Value.Region;
            return regionName == null ?
                RegionEndpoint.USEast1 : // Config Default -> Great Idea!!
                RegionEndpoint.GetBySystemName(regionName);
        }

        private static async Task SendAwsEmail(IAmazonSimpleEmailService client, SendEmailRequest request)
        {
            try
            {
                var _ = await client.SendEmailAsync(request);
            }
            catch (AmazonSimpleEmailServiceException ex)
            {
                throw new ServiceException("Sending email via AWS SES failed.", ex);
            }
        }

        private static SendEmailRequest BuildSendEmailRequest(MailMessage email)
        {
            return new SendEmailRequest
            {
                Source = email.From?.Address,
                Destination = new Destination
                {
                    ToAddresses = [email.To[0].Address]
                },
                Message = new Message
                {
                    Subject = new Content(email.Subject),
                    Body = new Body
                    {
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = email.Body
                        }
                    }
                }
            };
        }
    }
}