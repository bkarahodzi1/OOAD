using System;

namespace BrainBoost.Models
{
    public class CourseProgress
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public DateTime? LastAccess { get; set; }
        public double Progress { get; set; }
        public bool IsCompleted { get; set; }
        public int Hours { get; set; }
    }
}
}
