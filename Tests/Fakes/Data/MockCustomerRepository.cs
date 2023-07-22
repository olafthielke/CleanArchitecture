using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using FluentAssertions;

namespace Tests.Fakes.Data
{
    /// <summary>
    /// Hand-rolled spy or mock implementation for unit testing
    /// ICustomerRepository calls.
    /// </summary>
    public class MockCustomerRepository : ICustomerRepository
    {
        public bool WasGetCustomerCalled;
        public bool WasSaveCustomerCalled;

        public string PassedInEmailAddress;
        public Customer PassedInCustomer;

        public Customer CustomerToBeReturned;


        public MockCustomerRepository(Customer customer)
        {
            CustomerToBeReturned = customer;
        }


        public async Task<Customer> GetCustomer(string emailAddress)
        {
            await Task.CompletedTask;
            WasGetCustomerCalled = true;
            PassedInEmailAddress = emailAddress;
            return CustomerToBeReturned;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            await Task.CompletedTask;
            return new List<Customer>(); // TODO: Make this work.
        }

        public async Task SaveCustomer(Customer customer)
        {
            await Task.CompletedTask;
            WasSaveCustomerCalled = true;
            PassedInCustomer = customer;
        }

        public void VerifySaveCustomerCall(Customer customer)
        {
            WasSaveCustomerCalled.Should().BeTrue();
            PassedInCustomer.Should().BeEquivalentTo(customer);
        }
    }
}