using BusinessLogic.Entities;
using System.ComponentModel.DataAnnotations;

namespace Data.Postgres
{
    public class DbCustomer
    {
        public int id { get; set; }
        public Guid guid { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email_address { get; set; }

        public DbCustomer()
        { }

        public DbCustomer(Customer customer)
        {
            guid = customer.Id;
            first_name = customer.FirstName;
            last_name = customer.LastName;
            email_address = customer.EmailAddress;
        }

        public Customer ToCustomer()
        {
            return new Customer(guid, first_name, last_name, email_address);
        }
    }
}
