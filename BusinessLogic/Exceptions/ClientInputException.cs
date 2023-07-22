using System;

namespace BusinessLogic.Exceptions
{
    public abstract class ClientInputException : Exception
    {
        protected ClientInputException()
        { }

        protected ClientInputException(string message)
            : base(message)
        { }
    }
}