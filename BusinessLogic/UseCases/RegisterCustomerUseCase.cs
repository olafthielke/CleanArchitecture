using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.UseCases
{
    public class RegisterCustomerUseCase(ICustomerRepository repository, ICustomerNotifier notifier)
        : IRegisterCustomerUseCase
    {
        public ICustomerRepository Repository { get; } = repository;
        public ICustomerNotifier Notifier { get; } = notifier;


        public async Task<Result<Customer, Error>> RegisterCustomer(CustomerRegistration reg)
        {
            var validationResult = await Validate(reg);
            if (validationResult.IsError)
                return validationResult.Error;

            var customer = reg.ToCustomer();
            await Repository.SaveCustomer(customer);
            await Notifier.SendWelcomeMessage(customer);
            return customer;
        }


        private async Task<Result<bool, Error>> Validate(CustomerRegistration reg)
        {
            if (reg == null)
                return ValidationErrors.MissingCustomerRegistration;
            var validationResult = reg.Validate();
            if (validationResult.IsError)
                return validationResult;
            var existCust = await Repository.GetCustomer(reg.EmailAddress);
            if (existCust != null)
                return ValidationErrors.DuplicateCustomerEmailAddress;

            return true;
        }
    }
}