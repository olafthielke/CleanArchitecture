using System.Threading.Tasks;
using Notification.SMS.Models;

namespace Notification.SMS.Interfaces
{
    public interface ISmsSender
    {
        Task Send(SmsMessage message);
    }
}
