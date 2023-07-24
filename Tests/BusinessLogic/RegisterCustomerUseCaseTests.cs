using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using BusinessLogic.UseCases;
using Tests.Fakes.Data;

namespace Tests.BusinessLogic
{
    /// <summary>
    /// Unit tests for the RegisterCustomerUseCase class.
    /// Unit test names are in the Given_When_Then style and thus clearly
    /// describe business requirements.
    /// It's imperative to keep unit tests as concise as possible. Preferably
    /// just 3 lines. Unit tests should be even clearer than the implementation
    /// code.
    /// </summary>
    public class RegisterCustomerUseCaseTests
    {
        [Fact]
        public async Task Given_No_CustomerRegistration_When_Call_RegisterCustomer_Then_Throw_MissingCustomerRegistration()
        {
            var useCase = SetupUseCase();
            var register = () => useCase.RegisterCustomer(null);
            await register.Should().ThrowExactlyAsync<MissingCustomerRegistration>()
                          .Where(x => x.Message == "Missing customer registration data.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(" \r\n  ")]
        public async Task Given_Missing_FirstName_When_Call_RegisterCustomer_Then_Throw_MissingFirstName(string firstName)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration(firstName, "Smith", "bob@smith.com");
            var register = () => useCase.RegisterCustomer(registration);
            await register.Should().ThrowAsync<MissingFirstName>()
                          .Where(x => x.Message == "Missing first name.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("  \t \v ")]
        public async Task Given_Missing_LastName_When_Call_RegisterCustomer_Then_Throw_MissingLastName(string lastName)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration("Bob", lastName, "bob@smith.com");
            var register = () => useCase.RegisterCustomer(registration);
            await register.Should().ThrowAsync<MissingLastName>()
                          .Where(x => x.Message == "Missing last name.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        [InlineData(" \r\n \t \v ")]
        public async Task Given_Missing_EmailAddress_When_Call_RegisterCustomer_Then_Throw_MissingEmailAddress(string emailAddress)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration("Bob", "Smith", emailAddress);
            var register = () => useCase.RegisterCustomer(registration);
            await register.Should().ThrowAsync<MissingEmailAddress>()
                          .Where(x => x.Message == "Missing email address.");
        }

        [Theory]
        [MemberData(nameof(GetRegistrations))]
        public async Task When_Call_RegisterCustomer_Then_Lookup_Customer_By_Email_Address_In_Repository(CustomerRegistration registration)
        {
            var useCase = SetupUseCase();
            await useCase.RegisterCustomer(registration);
            VerifyCallRepositoryGetCustomerByEmailAddress(useCase, registration);
        }

        [Theory]
        [MemberData(nameof(GetRegistrationsWithCustomers))]
        public async Task Given_Customer_EmailAddress_Already_Exists_In_Repository_When_Call_RegisterCustomer_Then_Throw_DuplicateCustomerEmailAddress(CustomerRegistration registration,
            Customer customer)
        {
            var useCase = SetupUseCase(customer);
            var register = () => useCase.RegisterCustomer(registration);
            await register.Should().ThrowAsync<DuplicateCustomerEmailAddress>()
                          .Where(x => x.EmailAddress == customer.EmailAddress)
                          .Where(x => x.Message == $"The email address '{customer.EmailAddress}' already exists in the system.");
        }

        [Theory]
        [MemberData(nameof(GetRegistrations))]
        public async Task Given_Customer_EmailAddress_Is_Not_In_Repository_When_Call_RegisterCustomer_Then_Convert_Registration_To_Customer_And_Return_Customer(CustomerRegistration registration)
        {
            var useCase = SetupUseCase();
            var customer = await useCase.RegisterCustomer(registration);
            VerifyConvertRegistrationToCustomer(registration, customer);
        }

        [Theory]
        [MemberData(nameof(GetRegistrations))]
        public async Task Given_New_Customer_When_Call_RegisterCustomer_Then_Save_Customer_To_Repository(CustomerRegistration registration)
        {
            var useCase = SetupUseCase();
            var customer = await useCase.RegisterCustomer(registration);
            VerifySaveCustomerToRepository(useCase, customer);
        }


        private static readonly CustomerRegistration RegoAdamAnt = new CustomerRegistration("Adam", "Ant", "adam@ant.co.uk");
        private static readonly CustomerRegistration RegoBobSmith = new CustomerRegistration("Bob", "Smith", "bob@smith.com");


        public static IEnumerable<object[]> GetRegistrations()
        {
            yield return new object[] { RegoAdamAnt };
            yield return new object[] { RegoBobSmith };
        }

        private static readonly Customer CustomerAdamAnt = new Customer(Guid.NewGuid(), "Adam", "Ant", "adam@ant.co.uk");
        private static readonly Customer CustomerBobSmith = new Customer(Guid.NewGuid(), "Bob", "Smith", "bob@smith.com");

        public static IEnumerable<object[]> GetRegistrationsWithCustomers()
        {
            yield return new object[] { RegoAdamAnt, CustomerAdamAnt };
            yield return new object[] { RegoBobSmith, CustomerBobSmith };
        }


        private static RegisterCustomerUseCase SetupUseCase(Customer customer = null)
        {
            var repository = new MockCustomerRepository(customer);
            return new RegisterCustomerUseCase(repository);
        }


        private static void VerifyCallRepositoryGetCustomerByEmailAddress(RegisterCustomerUseCase useCase,
            CustomerRegistration registration)
        {
            var repository = (MockCustomerRepository)useCase.Repository;
            repository.WasGetCustomerCalled.Should().BeTrue();
            repository.PassedInEmailAddress.Should().Be(registration.EmailAddress);
        }

        private static void VerifyConvertRegistrationToCustomer(CustomerRegistration registration, Customer customer)
        {
            customer.FirstName.Should().Be(registration.FirstName);
            customer.LastName.Should().Be(registration.LastName);
            customer.EmailAddress.Should().Be(registration.EmailAddress);
        }

        private static void VerifySaveCustomerToRepository(RegisterCustomerUseCase useCase, Customer customer)
        {
            var repository = (MockCustomerRepository)useCase.Repository;
            repository.WasSaveCustomerCalled.Should().BeTrue();
            repository.PassedInCustomer.Should().BeEquivalentTo(customer);
        }
    }
}