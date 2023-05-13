using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class CourseProgress
    {
        [Key]
        public int CourseProgressId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime? LastAccess { get; set; }
        public double Progress { get; set; }
        public bool IsCompleted { get; set; }
        public int Hours { get; set; }

        public CourseProgress() { }
    }
}
