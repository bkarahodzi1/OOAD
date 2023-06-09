using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Display(Name = "Rate")]
        public int Rate { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        public Feedback () { }
        public static Feedback CreateFromRate(int rate)
        {
            return new Feedback
            {
                Rate = rate,
                CreatedAt = DateTime.Now
            };
        }

        public static Feedback CreateFromComment(string comment)
        {
            return new Feedback
            {
                Comment = comment,
                CreatedAt = DateTime.Now
            };
        }
    }
}
