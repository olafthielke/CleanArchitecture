using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Data.Proxy
{
    public class NullCustomerCache : ICustomerCache
    {
        public async Task<Customer> GetCustomer(string emailAddress)
        {
            return null;
        }


        public async Task SaveCustomer(Customer customer)
        {
            // Do Nothing
        }
    }
}
