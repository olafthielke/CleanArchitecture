namespace BusinessLogic.Exceptions
{
    public class MissingEmailAddress : ClientInputException
    {
        public MissingEmailAddress()
            : base("Missing email address.")
        { }
    }
}