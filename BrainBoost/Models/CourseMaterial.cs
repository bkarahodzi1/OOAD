using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class CourseMaterial
    {
        [Key]
        public int CourseMaterialId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "File")]
        public string File { get; set; }

        [Display(Name = "File type")]
        public FileType FileType { get; set; }

        [Display(Name = "View count")]
        public int ViewCount { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated at")]
        public DateTime UpdatedAt { get; set; }

        public CourseMaterial () { }
    }
}

