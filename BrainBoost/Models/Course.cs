using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        [Display(Name = "Course name")]
        [Required(ErrorMessage = "Course name is required.")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public double? Price { get; set; }
        [Required(ErrorMessage = "Currency is required.")]
        public string Currency { get; set; }
        public int CompletedCount { get; set; }
        public double CompletedPercentage { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Course() { }
    }
}
