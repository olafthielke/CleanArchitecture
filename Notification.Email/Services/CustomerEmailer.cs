using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Notification.Email.Services
{
    public class CustomerEmailer : ICustomerNotifier
    {
        public async Task SendWelcomeMessage(Customer customer)
        {
            // TODO
        }
    }
}
