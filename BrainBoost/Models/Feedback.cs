using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Feedback () { }
    }
}
