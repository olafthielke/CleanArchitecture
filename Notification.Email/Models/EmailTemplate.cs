
namespace Notification.Email.Models
{
    public record EmailTemplate
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailTemplate(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
