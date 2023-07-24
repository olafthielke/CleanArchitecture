using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
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
        public void Given_No_CustomerRegistration_When_Call_RegisterCustomer_Then_Throw_MissingCustomerRegistration()
        {
            var useCase = SetupUseCase();
            Func<Task> register = async () => { await useCase.RegisterCustomer(null); };
            register.Should().ThrowAsync<MissingCustomerRegistration>()
                             .Where(x => x.Message == "Missing customer registration data.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData(" \r\n  ")]
        public void Given_Missing_FirstName_When_Call_RegisterCustomer_Then_Throw_MissingFirstName(string firstName)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration(firstName, "Smith", "bob@smith.com");
            Func<Task> register = async () => { await useCase.RegisterCustomer(registration); };
            register.Should().ThrowAsync<MissingFirstName>()
                             .Where(x => x.Message == "Missing first name.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("  \t \v ")]
        public void Given_Missing_LastName_When_Call_RegisterCustomer_Then_Throw_MissingLastName(string lastName)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration("Bob", lastName, "bob@smith.com");
            Func<Task> register = async () => { await useCase.RegisterCustomer(registration); };
            register.Should().ThrowAsync<MissingLastName>()
                             .Where(x => x.Message == "Missing last name.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        [InlineData(" \r\n \t \v ")]
        public void Given_Missing_EmailAddress_When_Call_RegisterCustomer_Then_Throw_MissingEmailAddress(string emailAddress)
        {
            var useCase = SetupUseCase();
            var registration = new CustomerRegistration("Bob", "Smith", emailAddress);
            Func<Task> register = async () => { await useCase.RegisterCustomer(registration); };
            register.Should().ThrowAsync<MissingEmailAddress>()
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

        [Fact]
        public void Given_Customer_With_EmailAddress_Already_Exists_When_Call_RegisterCustomer_Then_Throw_DuplicateCustomerEmailAddress()
        {
            var database = new InMemoryCustomerDatabase();
            database.SaveCustomer(FredFlintstone).Wait();
            var useCase = new RegisterCustomerUseCase(database);

            Func<Task> register = async () => { await useCase.RegisterCustomer(FredFlintstoneRego); };

            register.Should().ThrowAsync<DuplicateCustomerEmailAddress>()
                             .Where(x => x.EmailAddress == FredFlintstone.EmailAddress)
                             .Where(x => x.Message == $"The email address '{FredFlintstone.EmailAddress}' already exists in the system.");
        }

        [Theory]
        [MemberData(nameof(GetRegistrationsWithCustomers))]
        public void Given_Customer_EmailAddress_Already_Exists_In_Repository_When_Call_RegisterCustomer_Then_Throw_DuplicateCustomerEmailAddress(CustomerRegistration registration,
            Customer customer)
        {
            var useCase = SetupUseCase(customer);
            Func<Task> register = async () => { await useCase.RegisterCustomer(registration); };
            register.Should().ThrowAsync<DuplicateCustomerEmailAddress>()
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

        [Theory]
        [MemberData(nameof(GetRegistrations))]
        public async Task When_Call_RegisterCustomer_Then_Save_Customer_To_Repository(CustomerRegistration registration)
        {
            var repository = new MockCustomerRepository(null);
            var useCase = new RegisterCustomerUseCase(repository);
            var customer = await useCase.RegisterCustomer(registration);
            repository.VerifySaveCustomerCall(customer);
        }


        private static readonly CustomerRegistration FredFlintstoneRego = new CustomerRegistration("Fred", "Flintstone", "fred@flintstone.net");
        private static readonly Customer FredFlintstone = new Customer(Guid.NewGuid(), "Fred", "Flintstone", "fred@flintstone.net");

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