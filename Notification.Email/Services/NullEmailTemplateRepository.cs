using System.Threading.Tasks;
using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class NullEmailTemplateRepository : IEmailTemplateRepository
    {
        public async Task<EmailTemplate> Get(string templateName)
        {
            return null;
        }
    }
}
