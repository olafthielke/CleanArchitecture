using System;
using System.Threading.Tasks;
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
        public Exception ExceptionToThrow;


        public MockRegisterCustomerUseCase(Customer customer = null)
        {
            CustomerToReturn = customer;
        }

        public MockRegisterCustomerUseCase(Exception exception)
        {
            ExceptionToThrow = exception;
        }


        public async Task<Customer> RegisterCustomer(CustomerRegistration registration)
        {
            await Task.CompletedTask;
            WasRegisterCalled = true;
            PassedInRegistration = registration;
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;
            return CustomerToReturn;
        }
    }
}