using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using Notification.Common.Interfaces;
using Notification.SMS.Exceptions;
using Notification.SMS.Interfaces;
using Notification.SMS.Models;

namespace Notification.SMS.Services
{
    public class CustomerSmsSender(ISmsTemplateRepository smsTemplateRepo,
        SmsConfiguration config,
        IPlaceholderReplacer replacer,
        ISmsSender smsSender) : ICustomerNotifier
    {
        private ISmsTemplateRepository SmsTemplateRepo { get; } = smsTemplateRepo;
        private SmsConfiguration Config { get; } = config;
        private IPlaceholderReplacer Replacer { get; } = replacer;
        private ISmsSender SmsSender { get; } = smsSender;

        private string FromNumber
        {
            get
            {
                if (Config.FromNumber == null)
                    throw new MissingFromMobileNumber();
                return Config.FromNumber;
            }
        }

        public async Task SendWelcomeMessage(Customer customer)
        {
            var template = await GetSmsTemplate("Customer Welcome");
            var message = BuildSmsMessage(template, customer);
            await SmsSender.Send(message);
        }


        private async Task<SmsTemplate> GetSmsTemplate(string templateName)
        {
            var template = await SmsTemplateRepo.GetSmsTemplate(templateName);
            if (template == null)
                throw new MissingSmsTemplate(templateName);
            return template;
        }

        private SmsMessage BuildSmsMessage(SmsTemplate template, Customer customer)
        {
            var message = Replacer.Replace(template.Message, customer);
            return new SmsMessage(FromNumber, customer.MobileNumber, message);
        }
    }
}
