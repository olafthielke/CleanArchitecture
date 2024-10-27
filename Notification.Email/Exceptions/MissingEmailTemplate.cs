using System;

namespace Notification.Email.Exceptions
{
    public class MissingEmailTemplate(string templateName)
        : Exception($"No email template with name '{templateName}' was found.");
}
