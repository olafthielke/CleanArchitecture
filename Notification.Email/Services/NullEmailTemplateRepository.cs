using System.Threading.Tasks;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class NullEmailTemplateRepository : IEmailTemplateRepository
    {
        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
