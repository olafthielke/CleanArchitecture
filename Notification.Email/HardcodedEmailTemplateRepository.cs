using System;
using System.Threading.Tasks;
using Notification.Email.Interfaces;

namespace Notification.Email
{
    public class HardcodedEmailTemplateRepository : IEmailTemplateRepository
    {
        private const string CUSTOMER_WELCOME_SUBJECT = "Welcome to ABC Corp!";

        private const string CUSTOMER_WELCOME_BODY = @"
            < html>
                <head></head>
                <body>
                    <h1>Amazon SES Test (AWS SDK for .NET)</h1>
                    <p>This email was sent with
                        <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
                        <a href='https://aws.amazon.com/sdk-for-net/'>
                        AWS SDK for .NET</a>.</p>
                </body>
            </html>";


        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            return new EmailTemplate(CUSTOMER_WELCOME_SUBJECT, CUSTOMER_WELCOME_BODY);
        }
    }
}
