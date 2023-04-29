using System.Collections.Generic;
using System;

namespace BrainBoost.Models
{
    public class Course
    {
        public string Id { get; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Currency { get; set; }
        public Professor Professor { get; set; }
        public List<Student> Attendees { get; set; }
        public int CompletedCount { get; set; }
        public double CompletedPercentage { get; set; }
        public List<CourseMaterial> Materials { get; set; }
        public List<Quiz> AllQuizes { get; set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; set; }
    }
}
