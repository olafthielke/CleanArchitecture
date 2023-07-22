using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace BusinessLogic.UseCases
{
    public class EditCustomerUseCase : IEditCustomerUseCase
    {
        // Work in Progress

        public ICustomerRepository Repository { get; }


        public EditCustomerUseCase(ICustomerRepository repository)
        {
            Repository = repository;
        }


        public async Task EditCustomer(CustomerModification mod)
        {
            await Validate(mod);

            // TODO
        }


        private async Task Validate(CustomerModification mod)
        {
            // TODO
        }
    }
}