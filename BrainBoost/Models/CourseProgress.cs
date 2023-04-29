using System;

namespace BrainBoost.Models
{
    public class CourseProgress
    {
        public string Id { get;  }
        public string StudentId { get; }
        public string CourseId { get; }
        public DateTime? LastAccess { get; set; }
        public double Progress { get; set; }
        public bool IsCompleted { get; set; }
        public int Hours { get; set; }
    }
}
}
