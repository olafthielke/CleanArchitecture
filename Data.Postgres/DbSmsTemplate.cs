using System.ComponentModel.DataAnnotations;
using Notification.SMS.Models;

namespace Data.Postgres
{
    public class DbSmsTemplate
    {
        [Key]
        public string name { get; set; }
        [Required]
        public string message { get; set; }


        public SmsTemplate ToSmsTemplate()
        {
            return new SmsTemplate(name, message);
        }
    }
}
