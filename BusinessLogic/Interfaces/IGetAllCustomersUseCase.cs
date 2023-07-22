using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IGetAllCustomersUseCase
    {
        Task<IEnumerable<Customer>> GetAllCustomers();
    }
}