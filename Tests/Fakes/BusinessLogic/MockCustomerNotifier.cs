using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Tests.Fakes.BusinessLogic
{
    public class MockCustomerNotifier : ICustomerNotifier
    {
        public bool WasSendWelcomeMessageCalled;
        public Customer PassedInCustomer;


        public async Task SendWelcomeMessage(Customer customer)
        {
            await Task.CompletedTask;
            WasSendWelcomeMessageCalled = true;
            PassedInCustomer = customer;
        }
    }
}
