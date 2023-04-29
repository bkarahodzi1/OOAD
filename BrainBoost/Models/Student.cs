using System.Collections.Generic;

namespace BrainBoost.Models
{
    public class Student : User
    {
        public List<Course> SubscribedCourses { get; set; }
        public double AccountBalance { get; set; }
        public Dictionary<string, int> Points { get; set; }

    }
}
