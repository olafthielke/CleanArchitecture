using System.Threading.Tasks;

namespace Notification.Email.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetEmailTemplate(string templateName);    // TODO: Add templateType, E.g. plain text, HTML
    }
}
