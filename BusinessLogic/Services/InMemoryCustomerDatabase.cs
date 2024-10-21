using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Services
{
    public class InMemoryCustomerDatabase : ICustomerDatabase, ICustomerCache
    {
        private static List<Customer> Customers { get; } = [];

        public async Task<Customer> GetCustomer(string emailAddress)
        {
            await Task.CompletedTask;
            return Customers.FirstOrDefault(c => c.EmailAddress == emailAddress);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.CompletedTask;
            return Customers;
        }

        public async Task SaveCustomer(Customer customer)
        {
            await Task.CompletedTask;
            Customers.Add(customer);
        }
    }
}
