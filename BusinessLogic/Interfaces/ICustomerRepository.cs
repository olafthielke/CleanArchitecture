using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(string emailAddress);

        Task<IEnumerable<Customer>> GetAllCustomers();

        Task SaveCustomer(Customer customer);
    }
}