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

        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string File { get; set; }
        public FileType FileType { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CourseMaterial () { }
    }
}

