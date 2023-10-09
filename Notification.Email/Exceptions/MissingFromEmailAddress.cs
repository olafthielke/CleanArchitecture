using System;

namespace Notification.Email.Exceptions
{
    public class MissingFromEmailAddress : Exception
    {
        public MissingFromEmailAddress() 
            : base("No valid FromEmailAddress was found.")
        { }
    }
}
