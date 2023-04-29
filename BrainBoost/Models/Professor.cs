using System.Collections.Generic;

namespace BrainBoost.Models
{
    public class Professor : User
    {
        public List<Course> CreatedCourses { get; set; }

    }
}
