using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Notification.Email.Interfaces;

namespace Notification.Email
{
    public class CustomerEmailer : ICustomerNotifier
    {
        // Work in Progress

        private IEmailer Emailer { get; }
        private IEmailConfiguration Config { get;}
        private IEmailTemplateRepository Repository { get; }


        public CustomerEmailer(IEmailer emailer, 
            IEmailConfiguration config, 
            IEmailTemplateRepository repository)
        {
            Emailer = emailer;
            Config = config;
            Repository = repository;
        }


        public async Task SendWelcomeMessage(Customer customer)
        {
            var template = await Repository.GetEmailTemplate("Customer Welcome");

            // TODO: Replace placeholders with customer info. 


            var email = new MailMessage(Config.FromEmailAddress, 
                customer.EmailAddress, 
                template.Subject, 
                template.Body);

            await Emailer.Send(email);
        }
    }
}
