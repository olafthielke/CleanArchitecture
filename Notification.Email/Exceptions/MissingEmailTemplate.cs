using System;

namespace Notification.Email.Exceptions
{
    public class MissingEmailTemplate : Exception
    {
        public MissingEmailTemplate(string templateName) 
            : base($"No email template with name '{templateName}' was found.")
        { }
    }
}
