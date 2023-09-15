using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;

namespace Notification.Email
{
    public class CustomerEmailer : ICustomerNotifier
    {
        // Work in Progress


        public async Task SendWelcomeMessage(Customer customer)
        {
            // TODO:
            // 1. Consider what you would have to do to write the code to send a Welcome email to the customer:
            //    a. What kind of information would you need?
            //    b. Where would that information come from?
            //    c. Which part of the overall behaviour in this method might you want to reuse in another place or program?
            //    d. Assume you don't want to store the email subject and body information in here. Where might that come from?
            //    e. Even more abstract, imagine we don't want to make any changes in here for any reasonable changes to email
            //       body/subject, from email address, how we send the emails, and so on. What should the code in here look like?
            //    f. What could go wrong? What kind of errors might we want to throw as exceptions?
        }
    }
}
