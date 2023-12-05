using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using System.Linq;

namespace Data.Postgres
{
    public class PostgresCustomerDatabase : ICustomerDatabase
    {
        private DataContext Context { get; set; }

        public PostgresCustomerDatabase(DataContext context)
        {
            Context = context;
        }


        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.CompletedTask;
            return (from dbCustomer in Context.customers
                    select dbCustomer.ToCustomer()).ToList();
        }

        public async Task<Customer> GetCustomer(string emailAddress)
        {
            await Task.CompletedTask;
            var dbCustomer = Context.customers.SingleOrDefault(c => c.email_address == emailAddress);
            if (dbCustomer == null)
                return null;
            return dbCustomer.ToCustomer();
        }

        public async Task SaveCustomer(Customer customer)
        {
            await Task.CompletedTask;
            var dbCustomer = new DbCustomer(customer);
            Context.customers.Add(dbCustomer);
            Context.SaveChanges();
        }
    }
}
