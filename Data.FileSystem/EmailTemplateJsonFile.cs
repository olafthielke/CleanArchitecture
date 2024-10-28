using System.Text.Json;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Data.FileSystem
{
    public class EmailTemplateJsonFile : IEmailTemplateRepository
    {
        private const string EmailTemplateFilePath = @"/Data/EmailTemplates.json";

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            var templates = await GetAllEmailTemplates();

            return templates.SingleOrDefault(c => c.Name == templateName);
        }


        private async Task<IEnumerable<EmailTemplate>> GetAllEmailTemplates()
        {
            var json = await File.ReadAllTextAsync(EmailTemplateFilePath);

            return JsonSerializer.Deserialize<IEnumerable<EmailTemplate>>(json);
        }
    }
}
