using Notification.Email.Models;

namespace Notification.Email.Interfaces
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate Get(string templateName);
    }
}
