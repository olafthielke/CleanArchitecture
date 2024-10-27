namespace Notification.SMS.Models
{
    public record SmsMessage(string From, string To, string Body)
    {
        public string From { get; set; } = From;
        public string To { get; set; } = To;
        public string Body { get; set; } = Body;
    }
}