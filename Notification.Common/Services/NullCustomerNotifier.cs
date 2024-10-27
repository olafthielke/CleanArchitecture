using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using System.Diagnostics;

namespace Notification.Common.Services
{
    public class NullCustomerNotifier : ICustomerNotifier
    {
        public async Task SendWelcomeMessage(Customer customer)
        {
            // Only log to console

            Debug.WriteLine($"\nSending Welcome message to customer '{customer.FirstName} {customer.LastName}'\n");
        }
    }
}
