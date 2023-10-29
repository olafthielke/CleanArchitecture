using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Notification.Email.Exceptions;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class CustomerEmailer : ICustomerNotifier
    {
        private IEmailTemplateRepository EmailTemplateRepo { get; }
        private IEmailConfiguration Config { get; }
        private IPlaceholderReplacer Replacer { get; }
        private IEmailer Emailer { get; }

        private string FromAddress
        {
            get
            {
                if (Config.FromAddress == null)
                    throw new MissingFromEmailAddress();
                return Config.FromAddress;
            }
        }


        public CustomerEmailer(IEmailTemplateRepository emailtemplateRepo,
            IEmailConfiguration config,
            IPlaceholderReplacer replacer,
            IEmailer emailer)
        {
            EmailTemplateRepo = emailtemplateRepo;
            Config = config;
            Replacer = replacer;
            Emailer = emailer;
        }


        public async Task SendWelcomeMessage(Customer customer)
        {
            var template = await GetEmailTemplate("Customer Welcome");
            var email = BuildEmail(template, customer);
            Emailer.Send(email);
        }


        private async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            var template = await EmailTemplateRepo.Get(templateName);
            if (template == null)
                throw new MissingEmailTemplate(templateName);
            return template;
        }

        private MailMessage BuildEmail(EmailTemplate template, Customer customer)
        {
            var subject = Replacer.Replace(template.Subject, customer);
            var body = Replacer.Replace(template.Body, customer);
            return new MailMessage(FromAddress,customer.EmailAddress, subject, body);
        }
    }
}
