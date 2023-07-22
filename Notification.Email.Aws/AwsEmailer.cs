using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Notification.Email.Interfaces;

namespace Notification.Email.Aws
{
    public class AwsEmailer : IEmailer
    {
        // Work in Progress

        public async Task Send(MailMessage message)
        {
            // TODO: Get the AWS Region from configuration.

            var region = RegionEndpoint.USWest2;
            using var client = new AmazonSimpleEmailServiceClient(region);
            var sendRequest = BuildEmailRequest(message);
            // TODO: Check for errors, throw exception.
            await client.SendEmailAsync(sendRequest);
        }


        private static SendEmailRequest BuildEmailRequest(MailMessage message)
        {
            // TODO: Can this be simplified?

            return new SendEmailRequest
            {
                Source = message.From.Address,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { message.To[0].Address }
                },
                Message = new Message
                {
                    Subject = new Content(message.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message.Body
                        }
                    }
                }
            };
        }
    }
}
