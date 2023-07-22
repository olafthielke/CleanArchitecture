namespace BusinessLogic.Exceptions
{
    public class MissingFirstName : ClientInputException
    {
        public MissingFirstName()
            : base("Missing first name.")
        { }
    }
}