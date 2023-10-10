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
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(null);
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, null, null, null);
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
            var mockReplacer = new Mock<IPlaceholderReplacer>();
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, mockReplacer.Object, null);
            var customer = new Customer();
            var send = () => customerEmailer.SendWelcomeMessage(customer);
            await send.Should().ThrowExactlyAsync<MissingFromEmailAddress>()
                .WithMessage("No valid FromEmailAddress was found.");
        }

        [Theory]
        [InlineData("sender@test.com", "fred@flintstones.net", "Welcome Fred!", "Hi Fred, ...")]
        [InlineData("from@test.com.au", "barney.rubble@gmail.com", "Welcome Barney!", "Hi Barney, ...")]
        public async Task When_Call_SendWelcomeMessage_Then_Send_Email(string fromAddress, string toAddress, string subject, string body)
        {
            // Arrange
            var template = new EmailTemplate("Welcome {{FirstName}}!", "Hi {{FirstName}}, ...");
            var customer = new Customer(Guid.NewGuid(), "Fred", "Flintstone", toAddress);
            var mockTemplateRepo = SetupMockEmailTemplateRepoToGet(template);
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress(fromAddress);
            var mockReplacer = SetupMockPlaceholderReplacer(template, customer, subject, body);
            var emailer = new Mock<IEmailer>();
            var customerEmailer = new CustomerEmailer(mockTemplateRepo.Object, mockEmailConfig.Object, mockReplacer.Object, emailer.Object);
            // Act
            await customerEmailer.SendWelcomeMessage(customer);
            // Assert
            emailer.Verify(e => e.Send(It.Is<MailMessage>(a => a.From.Address == fromAddress &&
                                                               a.To[0].Address == toAddress &&
                                                               a.Subject == subject &&
                                                               a.Body == body)));
        }


        private static Mock<IEmailTemplateRepository> SetupMockEmailTemplateRepoToGet(EmailTemplate template)
        {
            var mockEmailTemplateRepo = new Mock<IEmailTemplateRepository>();
            mockEmailTemplateRepo.Setup(x => x.Get("Customer Welcome"))
                .ReturnsAsync(template);

            return mockEmailTemplateRepo;
        }

        private static Mock<IEmailConfiguration> SetupMockEmailConfigToGetFromEmailAddress(string fromAddress)
        {
            var mockEmailConfig = new Mock<IEmailConfiguration>();
            mockEmailConfig.Setup(x => x.FromAddress).Returns(fromAddress);

            return mockEmailConfig;
        }

        private static Mock<IPlaceholderReplacer> SetupMockPlaceholderReplacer(EmailTemplate template, Customer customer, string subject,
            string body)
        {
            var mockReplacer = new Mock<IPlaceholderReplacer>();
            mockReplacer.Setup(r => r.Replace(template.Subject, customer)).Returns(subject);
            mockReplacer.Setup(r => r.Replace(template.Body, customer)).Returns(body);

            return mockReplacer;
        }
    }
}
