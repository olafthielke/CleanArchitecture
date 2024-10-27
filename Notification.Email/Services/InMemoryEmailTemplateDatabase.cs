using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class InMemoryEmailTemplateDatabase : IEmailTemplateRepository
    {
        private static List<EmailTemplate> EmailTemplates { get; } = [];

        public InMemoryEmailTemplateDatabase() { }

        public InMemoryEmailTemplateDatabase(EmailTemplate template)
        {
            EmailTemplates.Add(template);
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            await Task.CompletedTask;
            return EmailTemplates.FirstOrDefault(c => c.Name == templateName);
        }

        public async Task SaveEmailTemplate(EmailTemplate template)
        {
            await Task.CompletedTask;
            EmailTemplates.Add(template);
        }
    }
}
