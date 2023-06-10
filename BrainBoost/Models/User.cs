using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public abstract class User
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name must contain only letters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
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
