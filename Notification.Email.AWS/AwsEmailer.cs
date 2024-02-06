using System.Net.Mail;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BusinessLogic.Exceptions;
using Notification.Email.AWS.Interfaces;
using Notification.Email.Interfaces;

namespace Notification.Email.AWS
{
    public class AwsEmailer(
        IAmazonSimpleEmailServiceClientFactory clientFactory)
        : IEmailer
    { 
        private IAmazonSimpleEmailServiceClientFactory ClientFactory { get; } = clientFactory;

        public async Task Send(MailMessage email)
        {
            // Hardcoded AWS Region
            using var awsClient = ClientFactory.Create(RegionEndpoint.APSoutheast2);
            var request = BuildSendEmailRequest(email);
            await SendAwsEmail(awsClient, request);
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