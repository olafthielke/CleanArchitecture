namespace BusinessLogic.Exceptions
{
    public class MissingCustomerRegistration : ClientInputException
    {
        public MissingCustomerRegistration()
            : base("Missing customer registration data.")
        { }
    }
}