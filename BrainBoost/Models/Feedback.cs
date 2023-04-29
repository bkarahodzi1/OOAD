using System;

namespace BrainBoost.Models
{
    public class Feedback
    {
        public string Id { get;}
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
