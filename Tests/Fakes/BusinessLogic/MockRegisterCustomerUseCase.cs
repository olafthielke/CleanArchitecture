using System;
using System.Threading.Tasks;
using BusinessLogic;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Tests.Fakes.BusinessLogic
{
    /// <summary>
    /// Unit testing stand-in class for IRegisterCustomerUseCase.
    /// </summary>
    public class MockRegisterCustomerUseCase : IRegisterCustomerUseCase
    {
        public bool WasRegisterCalled;
        public CustomerRegistration PassedInRegistration;

        public Customer CustomerToReturn;
        public Error ErrorToReturn;
        public Exception ExceptionToThrow;

        public MockRegisterCustomerUseCase(Customer customer = null)
        {
            CustomerToReturn = customer;
        }

        public MockRegisterCustomerUseCase(Error error)
        {
            ErrorToReturn = error;
        }

        public MockRegisterCustomerUseCase(Exception ex)
        {
            ExceptionToThrow = ex;
        }

        public async Task<Result<Customer, Error>> RegisterCustomer(CustomerRegistration registration)
        {
            await Task.CompletedTask;
            WasRegisterCalled = true;
            PassedInRegistration = registration;
            if (ErrorToReturn != null)
                return ErrorToReturn;
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;
            return CustomerToReturn;
        }
    }
}