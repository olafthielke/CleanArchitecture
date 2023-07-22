namespace BusinessLogic.Exceptions
{
    public class MissingLastName : ClientInputException
    {
        public MissingLastName()
            : base("Missing last name.")
        { }
    }
}