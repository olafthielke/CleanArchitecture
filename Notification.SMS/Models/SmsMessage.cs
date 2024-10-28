namespace Notification.SMS.Models
{
    public class SmsMessage(string from, string to, string body)
    {
        public string From { get; } = from;
        public string To { get; } = to;
        public string Body { get; } = body;
    }
}