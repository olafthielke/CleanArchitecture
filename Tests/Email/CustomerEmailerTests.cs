using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using BusinessLogic.Entities;
using Notification.Email.Exceptions;
using Notification.Email.Interfaces;
using Notification.Email.Models;
using Notification.Email.Services;


namespace Tests.Email
{
    public class CustomerEmailerTests
    {
        [Fact]
        public async Task Given_EmailTemplate_Does_Not_Exist_When_Call_SendWelcomeMessage_Then_Throw_MissingEmailTemplate_Exception()
        {
            var customerEmailer = SetupCustomerEmailerWithoutEmailTemplate();
            Task Send() => customerEmailer.SendWelcomeMessage(new Customer());
            await AssertThrowsMissingEmailTemplate(Send);
        }

        [Fact]
        public async Task Given_FromEmailAddress_Does_Not_Exist_When_Call_SendWelcomeMessage_Then_Throw_MissingFromEmailAddress_Exception()
        {
            var customerEmailer = SetupCustomerEmailerWithoutFromEmailAddress();
            Task Send() => customerEmailer.SendWelcomeMessage(new Customer());
            await AssertThrowsMissingFromEmailAddress(Send);
        }


        [Theory]
        [InlineData("sender@test.com", "fred@flintstones.net")]
        [InlineData("donotreply@blah.mil", "fred.flintstones@outlook.com")]
        [InlineData("from@example.net", "fredflintstone@gmail.com")]
        public async Task When_Call_SendWelcomeMessage_Then_Send_Email(string fromAddress, string toAddress)
        {
            // TODO: More tidying up.
            // Arrange
            var customer = new Customer(Guid.NewGuid(), "Fred", "Flintstone", toAddress);
            var template = new EmailTemplate("Customer Welcome", "Welcome to XYZ Corp, {{FirstName}}!", "Hi {{FirstName}}, ...");
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGetEmailTemplate(template);
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

        private CustomerEmailer SetupCustomerEmailerWithoutEmailTemplate()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGetEmailTemplate(null);
            return new CustomerEmailer(mockEmailTemplateRepo.Object, null, null, null);
        }

        private CustomerEmailer SetupCustomerEmailerWithoutFromEmailAddress()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGetEmailTemplate(new EmailTemplate("Customer Welcome", "Subject", "Body"));
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress(null);
            var mockReplacer = new Mock<IPlaceholderReplacer>();
            return new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, mockReplacer.Object, null);
        }

        private static Mock<IEmailTemplateRepository> SetupMockEmailTemplateRepoToGetEmailTemplate(EmailTemplate template)
        {
            var mockEmailTemplateRepo = new Mock<IEmailTemplateRepository>();
            mockEmailTemplateRepo.Setup(x => x.GetEmailTemplate("Customer Welcome"))
                .ReturnsAsync(template);
            return mockEmailTemplateRepo;
        }

        private static Mock<IEmailConfiguration> SetupMockEmailConfigToGetFromEmailAddress(string fromAddress)
        {
            var mockEmailConfig = new Mock<IEmailConfiguration>();
            mockEmailConfig.Setup(x => x.FromAddress).Returns(fromAddress);
            return mockEmailConfig;
        }


        private static async Task AssertThrowsMissingEmailTemplate(Func<Task> send)
        {
            await send.Should().ThrowExactlyAsync<MissingEmailTemplate>()
                .WithMessage("No email template with name 'Customer Welcome' was found.");
        }

        private static async Task AssertThrowsMissingFromEmailAddress(Func<Task> send)
        {
            await send.Should().ThrowExactlyAsync<MissingFromEmailAddress>()
                .WithMessage("No valid FromEmailAddress was found.");
        }
    }
}
