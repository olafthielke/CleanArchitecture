using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Entities
{
    public class CustomerModification
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string EmailAddress { get; }

        public CustomerModification(Guid id, string firstName, string lastName, string emailAddress)
        {
            Id = id;
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