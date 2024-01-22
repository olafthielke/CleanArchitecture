using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IRegisterCustomerUseCase
    {
        Task<Result<Customer, Error>> RegisterCustomer(CustomerRegistration registration);
    }
}