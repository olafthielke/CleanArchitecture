using System.Net.Mail;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BusinessLogic.Exceptions;
using Notification.Email.AWS.Interfaces;
using Notification.Email.Interfaces;

namespace Notification.Email.AWS
{
    public class AwsEmailer : IEmailer
    { 
        private IAmazonSimpleEmailServiceClientFactory ClientFactory { get; }
        private IAmazonConfiguration Config { get; }

        public AwsEmailer(IAmazonSimpleEmailServiceClientFactory clientFactory, 
            IAmazonConfiguration config)
        {
            ClientFactory = clientFactory;
            Config = config;
        }

        public async Task Send(MailMessage email)
        {
            using var awsClient = ClientFactory.Create(Config.Region);
            var request = BuildSendEmailRequest(email);
            await SendAwsEmail(awsClient, request);
        }

        private static async Task SendAwsEmail(IAmazonSimpleEmailService client, SendEmailRequest request)
        {
            try
            {
                var response = await client.SendEmailAsync(request);
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
                    ToAddresses =
                        new List<string> { email.To[0].Address }
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