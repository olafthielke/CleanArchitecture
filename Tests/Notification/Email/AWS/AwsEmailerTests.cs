using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BusinessLogic.Exceptions;
using Notification.Email.AWS;
using Notification.Email.AWS.Interfaces;

namespace Tests.Notification.Email.AWS
{
    public class AwsEmailerTests
    {
        [Theory]
        [InlineData("a@b.c", "x@y.z", "subject", "body")]
        [InlineData("sender@example.com", "recipient@test.net", "This Is The Subject", "This Is The Body")]
        public async Task When_Call_Send_Then_Send_Email(string from, string to, string subject, string body)
        {
            var email = new MailMessage(from, to, subject, body);
            var mockAwsClient = new Mock<IAmazonSimpleEmailService>();
            var mockAwsClientFactory = new Mock<IAmazonSimpleEmailServiceClientFactory>();
            mockAwsClientFactory.Setup(x => x.Create()).Returns(mockAwsClient.Object);
            var emailer = new AwsEmailer(mockAwsClientFactory.Object);

            await emailer.Send(email);

            mockAwsClientFactory.Verify(f => f.Create());
            mockAwsClient.Verify(f => f.SendEmailAsync(It.Is<SendEmailRequest>(r => r.Source == from &&
                                                                                r.Destination.ToAddresses[0] == to &&
                                                                                r.Message.Subject.Data == subject &&
                                                                                r.Message.Body.Text.Data == body),
                                                                                default));
        }

        [Fact]
        public async Task Given_An_AmazonSimpleEmailServiceException_When_Call_Send_Then_Rethrow_As_ServiceException()
        {
            var email = new MailMessage("a@b.c", "x@y.z", "subject", "body");
            var mockAwsClient = new Mock<IAmazonSimpleEmailService>();
            mockAwsClient.Setup(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default))
                .Throws<MessageRejectedException>(() => throw new MessageRejectedException("Message Rejected!"));
            var mockAwsClientFactory = new Mock<IAmazonSimpleEmailServiceClientFactory>();
            mockAwsClientFactory.Setup(x => x.Create()).Returns(mockAwsClient.Object);
            var emailer = new AwsEmailer(mockAwsClientFactory.Object);

            Task Send() => emailer.Send(email);

            await AssertThrowsServiceException(Send);
        }


        private static async Task AssertThrowsServiceException(Func<Task> send)
        {
            await send.Should().ThrowAsync<ServiceException>()
                .WithMessage("Sending email via AWS SES failed.");
        }
    }
}
