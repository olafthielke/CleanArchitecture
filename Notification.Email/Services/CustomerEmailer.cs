using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Notification.Email.Exceptions;
using Notification.Email.Interfaces;

namespace Notification.Email.Services
{
    public class CustomerEmailer : ICustomerNotifier
    {
        // Work in Progress

        private IEmailTemplateRepository EmailTemplateRepo { get; }
        private IEmailConfiguration Config { get; }

        private IEmailer Emailer { get; }

        public CustomerEmailer(IEmailTemplateRepository emailtemplateRepo,
            IEmailConfiguration config,
            IEmailer emailer)
        {
            EmailTemplateRepo = emailtemplateRepo;
            Config = config;
            Emailer = emailer;
        }

        public async Task SendWelcomeMessage(Customer customer)
        {
            // TODO:
            // 1. Consider what you would have to do to write the code to send a Welcome email to the customer:
            //    a. What kind of information would you need?
            // customer firstname, email address, email body, email subject, from email address

            //    b. Where would that information come from?
            // customer parameter, email template (Customer Welcome), email config (from / sending email address)
            // "Hi {{FirstName}}," => "Hi Bob,"

            //    c. Which part of the overall behaviour in this method might you want to reuse in another place or program?
            // IEmailTemplateRepository, IEmailConfigRepository, IPlaceholderReplacer, IEmailer

            //    d. Assume you don't want to store the email subject and body information in here. Where might that come from?
            // SqlServerEmailTemplateDatabase

            //    e. Even more abstract, imagine we don't want to make any changes in here for any reasonable changes to email
            //       body/subject, from email address, how we send the emails, and so on. What should the code in here look like?

            //    f. What could go wrong? What kind of errors might we want to throw as exceptions?
            const string templateName = "Customer Welcome";

            var template = EmailTemplateRepo.Get(templateName);
            if (template == null)
                throw new MissingEmailTemplate(templateName);

            if (Config.FromAddress == null)
                throw new MissingFromEmailAddress();

            // email template => email

            Emailer.Send();
        }
    }
}
