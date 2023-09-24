using Notification.Email.Interfaces;

namespace Notification.Email.Services
{
    public class NullEmailTemplateRepository : IEmailTemplateRepository
    {
        public void Get(string templateName)
        {
            
        }
    }
}
