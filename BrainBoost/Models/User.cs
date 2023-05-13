using System;
using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public abstract class User
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }

        public User() { }
    }
}
