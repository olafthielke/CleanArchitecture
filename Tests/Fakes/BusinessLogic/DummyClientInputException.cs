using BusinessLogic.Exceptions;

namespace Tests.Fakes.BusinessLogic
{
    /// <summary>
    /// A derived ClientInputExcpetion class that only exists for
    /// unit testing a specific ClientInputException.
    /// </summary>
    public class DummyClientInputException : ClientInputException
    {
        public DummyClientInputException()
            : base()
        { }

        public DummyClientInputException(string message)
            : base(message)
        { }
    }
}