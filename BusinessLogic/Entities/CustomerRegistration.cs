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
            var errors = new ValidationException();
            
            if (string.IsNullOrWhiteSpace(FirstName))
                errors.Add("Missing first name.");
            if (string.IsNullOrWhiteSpace(LastName))
                errors.Add("Missing last name.");
            if (string.IsNullOrWhiteSpace(EmailAddress))
                errors.Add("Missing email address.");

            if (errors.HasErrors)
                throw errors;
        }

        public Customer ToCustomer()
        {
            return new Customer(Guid.NewGuid(), FirstName, LastName, EmailAddress);
        }
    }
}