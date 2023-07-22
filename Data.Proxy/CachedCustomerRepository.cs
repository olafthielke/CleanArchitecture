using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Data.Proxy
{
    public class CachedCustomerRepository : ICustomerRepository
    {
        private ICustomerDatabase Database { get; }
        private ICustomerCache Cache { get; }

        public CachedCustomerRepository(ICustomerDatabase database,
            ICustomerCache cache)
        {
            Cache = cache;
            Database = database;
        }

        public async Task<Customer> GetCustomer(string emailAddress)
        {
            var customer = await Cache.GetCustomer(emailAddress);
            if (customer != null)
                return customer;
            customer = await Database.GetCustomer(emailAddress);
            if (customer == null)
                return null;
            // Put the customer into the cache for future calls.
            await Cache.SaveCustomer(customer);
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            // No caching, since we are getting all (or more realistically, paged) customers.
            return await Database.GetAllCustomers();
        }

        public async Task SaveCustomer(Customer customer)
        {
            // TODO: Do this async.
            await Database.SaveCustomer(customer);
            await Cache.SaveCustomer(customer);
        }
    }
}
