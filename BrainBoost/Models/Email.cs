using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class Email
    {
        [Key]
        public int EmailId { get; set; }

        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Email () { }

    }
}
