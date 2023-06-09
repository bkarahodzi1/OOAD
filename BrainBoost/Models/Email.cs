using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class Email
    {
        [Key]
        public int EmailId { get; set; }

        [Display(Name = "Recipient email")]
        public string RecipientEmail { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        public Email () { }

    }
}
