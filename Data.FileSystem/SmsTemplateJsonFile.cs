using System.Text.Json;
using Notification.SMS.Interfaces;
using Notification.SMS.Models;

namespace Data.FileSystem
{
    public class SmsTemplateJsonFile : ISmsTemplateRepository
    {
        private const string SmsTemplateFilePath = @"/Data/SmsTemplates.json";

        public async Task<SmsTemplate> GetSmsTemplate(string templateName)
        {
            var templates = await GetAllSmsTemplates();

            return templates.SingleOrDefault(c => c.Name == templateName);
        }


        private async Task<IEnumerable<SmsTemplate>> GetAllSmsTemplates()
        {
            var json = await File.ReadAllTextAsync(SmsTemplateFilePath);

            return JsonSerializer.Deserialize<IEnumerable<SmsTemplate>>(json);
        }
    }
}
