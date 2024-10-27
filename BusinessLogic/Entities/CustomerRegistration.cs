using System;
using BusinessLogic.Exceptions;

namespace BusinessLogic.Entities
{
    public class CustomerRegistration(string firstName, string lastName, string emailAddress, string mobileNumber)
    {
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public string EmailAddress { get; } = emailAddress;
        public string MobileNumber { get; } = mobileNumber;


        public void Validate()
        {
            var errors = new ValidationException();
            
            if (string.IsNullOrWhiteSpace(FirstName))
                errors.Add("Missing first name.");
            if (string.IsNullOrWhiteSpace(LastName))
                errors.Add("Missing last name.");
            if (string.IsNullOrWhiteSpace(EmailAddress))
                errors.Add("Missing email address.");
            if (string.IsNullOrWhiteSpace(MobileNumber))
                errors.Add("Missing mobile number.");

            if (errors.HasErrors)
                throw errors;
        }

        public Customer ToCustomer()
        {
            return new Customer(Guid.NewGuid(), FirstName, LastName, EmailAddress, MobileNumber);
        }
    }
}