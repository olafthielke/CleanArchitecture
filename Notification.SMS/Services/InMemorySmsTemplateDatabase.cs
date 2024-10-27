using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.SMS.Interfaces;
using Notification.SMS.Models;

namespace Notification.SMS.Services
{
    public class InMemorySmsTemplateDatabase : ISmsTemplateRepository
    {
        private static List<SmsTemplate> SmsTemplates { get; } = [];

        public InMemorySmsTemplateDatabase() { }

        public InMemorySmsTemplateDatabase(SmsTemplate template)
        {
            SmsTemplates.Add(template);
        }

        public async Task<SmsTemplate> GetSmsTemplate(string templateName)
        {
            await Task.CompletedTask;
            return SmsTemplates.FirstOrDefault(c => c.Name == templateName);
        }

        public async Task SaveSmsTemplate(SmsTemplate template)
        {
            await Task.CompletedTask;
            SmsTemplates.Add(template);
        }
    }
}