using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class Student : User
    {

        [Display(Name = "Account balance")]
        public double AccountBalance { get; set; }

        public Student() { }

    }
}
