using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;


namespace BusinessLogic.UseCases
{
    public class GetAllCustomersUseCase(ICustomerRepository repository) : IGetAllCustomersUseCase
    {
        public ICustomerRepository Repository { get; } = repository;


        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await Repository.GetAllCustomers();
        }
    }
}
