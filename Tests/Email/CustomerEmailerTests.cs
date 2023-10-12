using System;
using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using Xunit;
using Moq;
using FluentAssertions;
using Notification.Email.Exceptions;
using Notification.Email.Interfaces;
using Notification.Email.Models;
using Notification.Email.Services;
using NuGet.Frameworks;


namespace Tests.Email
{
    public class CustomerEmailerTests
    {
        [Fact]
        public async Task Given_EmailTemplate_Does_Not_Exist_When_Call_SendWelcomeMessage_Then_Throw_MissingEmailTemplate_Exception()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(null);
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, null, null,null);
            var customer = new Customer();
            var send = () => customerEmailer.SendWelcomeMessage(customer);
            await send.Should().ThrowExactlyAsync<MissingEmailTemplate>()
                .WithMessage("No email template with name 'Customer Welcome' was found.");
        }

        [Fact]
        public async Task Given_FromEmailAddress_Does_Not_Exist_When_Call_SendWelcomeMessage_Then_Throw_MissingFromEmailAddress_Exception()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(new EmailTemplate("Subject", "Body"));
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress(null);
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, null,null);
            var customer = new Customer();
            var send = () => customerEmailer.SendWelcomeMessage(customer);
            await send.Should().ThrowExactlyAsync<MissingFromEmailAddress>()
                .WithMessage("No valid FromEmailAddress was found.");
        }

        [Theory]
        [InlineData("sender@test.com", "fred@flintstones.net")]
        [InlineData("donotreply@blah.mil", "fred.flintstones@outlook.com")]
        [InlineData("from@example.net", "fredflintstone@gmail.com")]
        public async Task When_Call_SendWelcomeMessage_Then_Send_Email(string fromAddress, string toAddress)
        {
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Fred", "Flintstone", toAddress);
            var template = new EmailTemplate("Welcome to XYZ Corp, {{FirstName}}!", "Hi {{FirstName}}, ...");
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(template);
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress(fromAddress);
            var mockReplacer = new Mock<IPlaceholderReplacer>();
            mockReplacer.Setup(r => r.Replace("Welcome to XYZ Corp, {{FirstName}}!", customer))
                .Returns("Welcome to XYZ Corp, Fred!");
            mockReplacer.Setup(r => r.Replace("Hi {{FirstName}}, ...", customer))
                .Returns("Hi Fred, ...");
            var emailer = new Mock<IEmailer>();
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, mockReplacer.Object, emailer.Object);
            // Act
            await customerEmailer.SendWelcomeMessage(customer);
            // Assert
            emailer.Verify(e => e.Send(It.Is<MailMessage>(m => m.From.Address == fromAddress &&
                                                               m.To[0].Address == toAddress &&
                                                               m.Subject == "Welcome to XYZ Corp, Fred!" &&
                                                               m.Body == "Hi Fred, ...")));
        }


        private static Mock<IEmailTemplateRepository> SetupMockEmailTemplateRepoToGet(EmailTemplate template)
        {
            var mockEmailTemplateRepo = new Mock<IEmailTemplateRepository>();
            mockEmailTemplateRepo.Setup(x => x.Get("Customer Welcome"))
                .Returns(template);
            return mockEmailTemplateRepo;
        }

        private static Mock<IEmailConfiguration> SetupMockEmailConfigToGetFromEmailAddress(string fromAddress)
        {
            var mockEmailConfig = new Mock<IEmailConfiguration>();
            mockEmailConfig.Setup(x => x.FromAddress).Returns(fromAddress);
            return mockEmailConfig;
        }


        // Body -> Template
        // Subject -> Template
        // Recipient Email Address -> Customer Email Address
        // Sender Email Address -> Config From Email Address
    }
}
