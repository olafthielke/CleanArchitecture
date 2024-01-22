using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;

namespace BusinessLogic.UseCases
{
    public class RegisterCustomerUseCase(ICustomerRepository repository, ICustomerNotifier notifier)
        : IRegisterCustomerUseCase
    {
        public ICustomerRepository Repository { get; } = repository;
        public ICustomerNotifier Notifier { get; } = notifier;


        public async Task<Customer> RegisterCustomer(CustomerRegistration reg)
        {
            await Validate(reg);
            var customer = reg.ToCustomer();
            await Repository.SaveCustomer(customer);
            await Notifier.SendWelcomeMessage(customer);
            return customer;
        }


        private async Task Validate(CustomerRegistration reg)
        {
            if (reg == null)
                throw new MissingCustomerRegistration();
            reg.Validate();
            var existCust = await Repository.GetCustomer(reg.EmailAddress);
            if (existCust != null)
                throw new DuplicateCustomerEmailAddress(reg.EmailAddress);
        }
    }
}