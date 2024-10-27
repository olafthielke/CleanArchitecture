using System;

namespace Notification.SMS.Exceptions
{
    public class MissingFromMobileNumber() : Exception("No valid FromMobileNumber was found.");
}
