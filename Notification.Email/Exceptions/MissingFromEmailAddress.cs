using System;

namespace Notification.Email.Exceptions
{
    public class MissingFromEmailAddress() : Exception("No valid FromEmailAddress was found.");
}
