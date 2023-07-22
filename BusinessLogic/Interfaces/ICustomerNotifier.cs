using System.Threading.Tasks;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface ICustomerNotifier
    {
        Task SendWelcomeMessage(Customer customer);
    }
}
