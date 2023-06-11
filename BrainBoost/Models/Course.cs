using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BrainBoost.Models
{
    public class Course : ICloneable
    {
        [Key]
        public int CourseId { get; set; }

        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }

        [Display(Name = "Course name")]
        [Required(ErrorMessage = "Course name is required.")]
        public string CourseName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Display(Name = "Price")]
        [DefaultValue(0)]
        public double? Price { get; set; }

        [Display(Name = "Currency")]
        [DefaultValue("KM")]
        public string Currency { get; set; }

        [Display(Name = "Completed count")]
        public int CompletedCount { get; set; }

        [Display(Name = "Completed percentage")]
        public double CompletedPercentage { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }

        public Course() { }

        public object Clone()
        {
            Course clonedCourse = new Course
            {
                CourseId = this.CourseId,
                ProfessorId = this.ProfessorId,
                Professor = this.Professor,
                CourseName = this.CourseName,
                Description = this.Description,
                Price = this.Price,
                Currency = this.Currency,
                CompletedCount = this.CompletedCount,
                CompletedPercentage = this.CompletedPercentage,
                CreatedAt = this.CreatedAt,
                UpdatedAt = this.UpdatedAt
            };

            return clonedCourse;
        }
    }
}
