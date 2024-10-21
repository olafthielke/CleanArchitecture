using System.Threading.Tasks;
using Notification.Email.Models;

namespace Notification.Email.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetEmailTemplate(string templateName);
    }
}
