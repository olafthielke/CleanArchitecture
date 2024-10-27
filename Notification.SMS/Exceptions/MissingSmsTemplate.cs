using System;

namespace Notification.SMS.Exceptions
{
    public class MissingSmsTemplate(string templateName)
        : Exception($"No SMS template with name '{templateName}' was found.");
}
