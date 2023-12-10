using Notification.Email.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Postgres
{
    public class DbEmailTemplate
    {
        [Key]
        public string name { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string body { get; set; }


        public EmailTemplate ToEmailTemplate()
        {
            return new EmailTemplate(name, subject, body);
        }
    }
}
