using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Entities
{
    public class CustomerRegistration
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string EmailAddress { get; }


        public CustomerRegistration(string firstName, string lastName, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new MissingFirstName();
            if (string.IsNullOrWhiteSpace(LastName))
                throw new MissingLastName();
            if (string.IsNullOrWhiteSpace(EmailAddress))
                throw new MissingEmailAddress();
        }

        public Customer ToCustomer()
        {
            return new Customer(Guid.NewGuid(), FirstName, LastName, EmailAddress);
        }
    }
}