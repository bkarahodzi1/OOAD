using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBoost.Models
{
    public class Billing
    {
        [ForeignKey("User")]
        public int BillingId { get; set; }
        public virtual User user { get; set; }

        [ForeignKey("BillingCard")]
        public int BillingCardId { get; set; }
        public virtual BillingCard BillingCard { get; set; }
        [NotMapped] // Exclude this property from database mapping
        public string CardNumber { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsPurchaseSuccessful { get; set; }

        public Billing() { }
    }

}
