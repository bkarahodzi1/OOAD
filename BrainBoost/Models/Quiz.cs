using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }

        [ForeignKey("CourseMaterial")]
        public int CourseMaterialId { get; set; }
        public CourseMaterial CourseMaterial { get; set; }

        [ForeignKey("User")]
        public int CreatedById { get; set; }

        [Display(Name = "Created by")]
        public User CreatedBy { get; set; }

        [Display(Name = "Quiz name")]
        public string QuizName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public Quiz() { }
    }
}

