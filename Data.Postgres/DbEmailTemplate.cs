using BusinessLogic.Entities;
using Notification.Email.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Data.Postgres
{
    public class DbEmailTemplate
    {
        public int id {  get; set; }
        [Required]
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
