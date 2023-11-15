namespace Notification.Email.Models
{
    public record EmailTemplate
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailTemplate(string name, string subject, string body)
        {
            Name = name;
            Subject = subject;
            Body = body;
        }
    }
}
