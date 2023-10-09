using Notification.Email.Interfaces;
using Notification.Email.Models;

namespace Notification.Email.Services
{
    public class NullEmailTemplateRepository : IEmailTemplateRepository
    {
        public EmailTemplate Get(string templateName)
        {
            return null;
        }
    }
}
