using System.Threading.Tasks;
using BusinessLogic.Entities;
using Xunit;
using Moq;
using FluentAssertions;
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
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, null, null);
            var customer = new Customer();
            var send = () => customerEmailer.SendWelcomeMessage(customer);
            await send.Should().ThrowExactlyAsync<MissingEmailTemplate>()
                .WithMessage("No email template with name 'Customer Welcome' was found.");
        }

        [Fact]
        public async Task Given_FromEmailAddress_Does_Not_Exist_When_Call_SendWelcomeMessage_Then_Throw_MissingFromEmailAddress_Exception()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(new EmailTemplate());
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress(null);
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, null);
            var customer = new Customer();
            var send = () => customerEmailer.SendWelcomeMessage(customer);
            await send.Should().ThrowExactlyAsync<MissingFromEmailAddress>()
                .WithMessage("No valid FromEmailAddress was found.");
        }

        [Fact]
        public async Task When_Call_SendWelcomeMessage_Then_Send_Email()
        {
            var mockEmailTemplateRepo = SetupMockEmailTemplateRepoToGet(new EmailTemplate());
            var mockEmailConfig = SetupMockEmailConfigToGetFromEmailAddress("sender@test.com");
            var emailer = new Mock<IEmailer>();
            var customerEmailer = new CustomerEmailer(mockEmailTemplateRepo.Object, mockEmailConfig.Object, emailer.Object);
            var customer = new Customer();
            await customerEmailer.SendWelcomeMessage(customer);
            emailer.Verify(x => x.Send());
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
