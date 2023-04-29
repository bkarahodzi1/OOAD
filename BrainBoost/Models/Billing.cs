using System;

namespace BrainBoost.Models
{
    public class Billing
    {
        public string SessionId { get; }
        public User User { get; set; }
        public BillingCard BillingCard { get; set; }
        public Course Course { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPurchaseSuccessful { get; set; }
    }

}
