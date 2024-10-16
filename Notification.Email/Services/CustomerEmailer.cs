using System.Net.Mail;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Notification.Email.Exceptions;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class CustomerEmailer(
        IEmailTemplateRepository emailTemplateRepo,
        IEmailConfiguration config,
        IPlaceholderReplacer replacer,
        IEmailer emailer)
        : ICustomerNotifier
    {
        private IEmailTemplateRepository EmailTemplateRepo { get; } = emailTemplateRepo;
        private IEmailConfiguration Config { get; } = config;
        private IPlaceholderReplacer Replacer { get; } = replacer;
        private IEmailer Emailer { get; } = emailer;

        private string FromAddress
        {
            get
            {
                if (Config.FromAddress == null)
                    throw new MissingFromEmailAddress();
                return Config.FromAddress;
            }
        }


        public async Task SendWelcomeMessage(Customer customer)
        {
            var template = await GetEmailTemplate("Customer Welcome");
            var email = BuildEmail(template, customer);
            await Emailer.Send(email);
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
