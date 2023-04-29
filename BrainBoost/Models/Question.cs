using System.Collections.Generic;
using System;

namespace BrainBoost.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string QuestionText { get; set; }
        public string Description { get; set; }
        public List<QuestionAnswer> Answers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    
}
}
