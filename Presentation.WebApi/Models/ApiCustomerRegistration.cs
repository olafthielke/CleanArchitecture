using BusinessLogic.Entities;

namespace Presentation.WebApi.Models
{
    public class ApiCustomerRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public ApiCustomerRegistration() { }

        public ApiCustomerRegistration(string firstName, string lastName, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }


        public CustomerRegistration ToRegistration()
        {
            return new CustomerRegistration(FirstName, LastName, EmailAddress);
        }
    }
}