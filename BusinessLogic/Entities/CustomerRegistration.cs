using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Entities
{
    public class CustomerRegistration(string firstName, string lastName, string emailAddress)
    {
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public string EmailAddress { get; } = emailAddress;


        public Result<bool, Error> Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return ValidationErrors.MissingFirstName;
            if (string.IsNullOrWhiteSpace(LastName))
                return ValidationErrors.MissingLastName;
            if (string.IsNullOrWhiteSpace(EmailAddress))
                return ValidationErrors.MissingEmailAddress;

            return true;
        }

        public Customer ToCustomer()
        {
            return new Customer(Guid.NewGuid(), FirstName, LastName, EmailAddress);
        }
    }
}