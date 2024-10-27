using Notification.SMS.Models;
using System.Threading.Tasks;

namespace Notification.SMS.Interfaces
{
    public interface ISmsTemplateRepository
    {
        Task<SmsTemplate?> GetSmsTemplate(string templateName);
    }
}
