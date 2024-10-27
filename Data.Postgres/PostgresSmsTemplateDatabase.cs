using Notification.SMS.Interfaces;
using Notification.SMS.Models;

namespace Data.Postgres
{
    public class PostgresSmsTemplateDatabase(DataContext context) : ISmsTemplateRepository
    {
        private DataContext Context { get; } = context;


        public async Task<SmsTemplate?> GetSmsTemplate(string templateName)
        {
            await Task.CompletedTask;
            var template = Context.sms_templates.SingleOrDefault(t => t.name == templateName);
            return template?.ToSmsTemplate();
        }
    }
}
