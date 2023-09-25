using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Services
{
    public class NullCustomerNotifier : ICustomerNotifier
    {
        public async Task SendWelcomeMessage(Customer customer)
        {
            // Do nothing
        }
    }
}
