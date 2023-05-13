using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class QuestionAnswer
    {
        [Key]
        public int QuestionAnswerId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string Answer { get; set; }
        public bool? IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public QuestionAnswer() { }
    }

}
