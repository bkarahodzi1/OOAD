using System;

namespace BrainBoost.Models
{
    public class Email
    {
        public string Id { get;}
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private set; }

    }
}
