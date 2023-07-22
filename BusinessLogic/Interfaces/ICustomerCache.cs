using BusinessLogic.Entities;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ICustomerCache
    {
        Task<Customer> GetCustomer(string emailAddress);

        Task SaveCustomer(Customer customer);
    }
}