using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Data.Postgres
{
    public class PostgresEmailTemplateDatabase(DataContext context) : IEmailTemplateRepository
    {
        private DataContext Context { get; } = context;


        public async Task<EmailTemplate> Get(string templateName)
        {
            await Task.CompletedTask;
            var template = Context.email_templates.SingleOrDefault(t => t.name == templateName);
            if (template == null)
                return null;
            return template.ToEmailTemplate();
        }
    }
}
