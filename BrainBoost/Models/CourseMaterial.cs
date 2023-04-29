using System.Collections.Generic;
using System;

namespace BrainBoost.Models
{
    public class CourseMaterial
    {
        public string Id { get; }
        public string CourseId { get;  }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string File { get; set; }
        public FileType FileType { get; set; }
        public List<Quiz> Quizes { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum FileType
    {
        PDF,DOCX,PNG,JPG,PPTX    
    }
}

