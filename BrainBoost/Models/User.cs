using System;

namespace BrainBoost.Models
{
    public abstract class User
    {
        public string Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; }
        public bool IsVerified { get; set; }
    }
}
