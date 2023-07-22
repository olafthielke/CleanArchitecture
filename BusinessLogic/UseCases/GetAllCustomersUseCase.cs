using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;


namespace BusinessLogic.UseCases
{
    public class GetAllCustomersUseCase : IGetAllCustomersUseCase
    {
        public ICustomerRepository Repository { get; }


        public GetAllCustomersUseCase(ICustomerRepository repository)
        {
            Repository = repository;
        }


        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            return await Repository.GetAllCustomers();
        }
    }
}
