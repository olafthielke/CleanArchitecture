using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using Moq;
using Notification.Email;
using Notification.Email.Interfaces;
using Xunit;

namespace Tests.Email
{
    public class CustomerEmailerTests
    {
        [Fact]
        public async Task When_Call_SendWelcomeMessage_Then_Try_Get_EmailTemplate()
        {
            var mockEmailTemplateRepo = new Mock<IEmailTemplateRepository>();
            var emailer = new CustomerEmailer(mockEmailTemplateRepo.Object);
            var customer = new Customer();
            await emailer.SendWelcomeMessage(customer);
            mockEmailTemplateRepo.Verify(x => x.Get("Customer Welcome"));
        }
    }
}
