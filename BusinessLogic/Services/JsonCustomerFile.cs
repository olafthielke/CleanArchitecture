using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.Services
{
    public class JsonCustomerFile : ICustomerDatabase, ICustomerCache
    {
        private const string CustomerFilePath = @"..\BusinessLogic\Services\Customers.json";

        public async Task<Customer> GetCustomer(string emailAddress)
        {
            var customers = await GetAllCustomers();

            return customers.SingleOrDefault(c => c.EmailAddress == emailAddress);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var json = await File.ReadAllTextAsync(CustomerFilePath);

            return JsonSerializer.Deserialize<IEnumerable<Customer>>(json);
        }


        public async Task SaveCustomer(Customer customer)
        {
            var customers = (await GetAllCustomers()).ToList();
            var existCust = customers.SingleOrDefault(c => c.Id == customer.Id);
            if (existCust == null)
                customers.Add(customer);
            // TODO: Otherwise Update existing customer.

            var json = JsonSerializer.Serialize(customers);
            await File.WriteAllTextAsync(CustomerFilePath, json);
        }
    }
}
