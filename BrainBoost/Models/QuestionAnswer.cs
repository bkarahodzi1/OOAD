using System;

namespace BrainBoost.Models
{
    public class QuestionAnswer
    {
        public string Id { get;  }
        public string Answer { get; set; }
        public bool? IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
