using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Amazon;
using Amazon.SimpleEmail;
using Notification.Email.AWS;
using Notification.Email.AWS.Interfaces;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Notification.Email.Services;
using Notification.Email.Exceptions;
using System;
using FluentAssertions;
using BusinessLogic.Exceptions;

namespace Tests.Email.AWS
{
    public class AwsEmailerTests
    {
        [Theory]
        [InlineData("a@b.c", "x@y.z", "subject", "body")]
        [InlineData("sender@example.com", "recipient@test.net", "This Is The Subject", "This Is The Body")]
        public async Task When_Call_Send_Then_Send_Email(string from, string to, string subject, string body)
        {
            var email = new MailMessage(from, to, subject, body);
            var mockAmazonConfig = new Mock<IAmazonConfiguration>();
            mockAmazonConfig.SetupGet(x => x.Region).Returns(RegionEndpoint.USEast1);
            var mockAwsClientFactory = new Mock<IAmazonSimpleEmailServiceClientFactory>();
            var mockAwsClient = new Mock<IAmazonSimpleEmailService>();
            mockAwsClientFactory.Setup(x => x.Create(It.IsAny<RegionEndpoint>()))
                .Returns(mockAwsClient.Object);
            var emailer = new AwsEmailer(mockAwsClientFactory.Object, mockAmazonConfig.Object);

            await emailer.Send(email);

            mockAwsClientFactory.Verify(f => f.Create(RegionEndpoint.USEast1));
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
            var mockAmazonConfig = new Mock<IAmazonConfiguration>();
            mockAmazonConfig.SetupGet(x => x.Region).Returns(RegionEndpoint.USEast1);
            var mockAwsClientFactory = new Mock<IAmazonSimpleEmailServiceClientFactory>();
            var mockAwsClient = new Mock<IAmazonSimpleEmailService>();
            mockAwsClient.Setup(x => x.SendEmailAsync(It.IsAny<SendEmailRequest>(), default))
                .Throws<MessageRejectedException>(() => throw new MessageRejectedException("Message Rejected!"));
            mockAwsClientFactory.Setup(x => x.Create(It.IsAny<RegionEndpoint>()))
                .Returns(mockAwsClient.Object);
            var emailer = new AwsEmailer(mockAwsClientFactory.Object, mockAmazonConfig.Object);

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
