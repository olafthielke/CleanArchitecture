namespace Notification.Email
{
    public class EmailTemplate
    {
        public string Subject { get; }
        public string Body { get; }

        public EmailTemplate(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
