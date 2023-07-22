using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IEditCustomerUseCase
    {
        Task EditCustomer(CustomerModification mod);
    }
}