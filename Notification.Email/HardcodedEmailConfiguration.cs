using Notification.Email.Interfaces;

namespace Notification.Email
{
    public class HardcodedEmailConfiguration : IEmailConfiguration
    {
        // Obviously we don't want to hard-code config details but at least by 
        // sequestering it in a separate file in the interim, we don't have to
        // change EXISTING code.
        // In the future, when we get this config from a file we will get to write
        // NEW code and not need to change existing code!  

        public string FromEmailAddress => "test@abc.com";
    }
}
