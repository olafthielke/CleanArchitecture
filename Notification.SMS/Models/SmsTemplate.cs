namespace Notification.SMS.Models
{
    public record SmsTemplate
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public SmsTemplate(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
