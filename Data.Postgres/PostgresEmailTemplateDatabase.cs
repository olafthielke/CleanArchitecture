using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Data.Postgres
{
    public class PostgresEmailTemplateDatabase : IEmailTemplateRepository
    {
        private DataContext Context { get; set; }

        public PostgresEmailTemplateDatabase(DataContext context)
        {
            Context = context;
        }


        public async Task<EmailTemplate> Get(string templateName)
        {
            await Task.CompletedTask;
            var template = Context.emailtemplates.SingleOrDefault(t => t.name == templateName);
            if (template == null)
                return null;
            return template.ToEmailTemplate();
        }
    }
}
