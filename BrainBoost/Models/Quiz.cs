using System.Collections.Generic;

namespace BrainBoost.Models
{
    public class Quiz
    {
        public int Id { get;  }
        public int CourseMaterialId { get;  }
        public string QuizName { get; set; }
        public string Description { get; set; }
        public User CreatedBy { get; set; }

        public List<Question> Questions { get; set; }
    }
}

