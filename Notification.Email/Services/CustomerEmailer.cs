using System;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Notification.Email.Services;

public class CustomerEmailer : ICustomerNotifier
{
    public async Task SendWelcomeMessage(Customer customer)
    {
        var subject = $"Welcome to XYZ, {customer.FirstName}!";

        var builder = new StringBuilder();

        builder.AppendLine($"Hi {customer.FirstName},");
        builder.AppendLine();
        builder.AppendLine("Thank you for signing up for the latest and greatest news from XYZ!");
        builder.AppendLine();
        builder.AppendLine($"Your customer number is {customer.Id}.");
        builder.AppendLine();
        builder.AppendLine("We hope to make your purchasing experience easy!");
        builder.AppendLine();
        builder.AppendLine("Kind Regards,");
        builder.AppendLine("The XYZ Customer Success Team");
        var body = builder.ToString();

        var fromAddress = "info@xyz.com";

        using var awsClient = new AmazonSimpleEmailServiceClient(RegionEndpoint.APSoutheast2);

        var email = new SendEmailRequest
        {
            Source = fromAddress,
            Destination = new Destination
            {
                ToAddresses = [customer.EmailAddress]
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = body
                    }
                }
            }
        };

        try
        {
            var response = await awsClient.SendEmailAsync(email);
        }
        catch (AmazonSimpleEmailServiceException ex)
        {
            Console.WriteLine("Sending email via AWS SES failed.");
        }
    }
}