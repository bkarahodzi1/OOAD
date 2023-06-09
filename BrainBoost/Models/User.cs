using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public abstract class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Is verified")]
        public bool IsVerified { get; set; }

        public User() { }
    }
}
