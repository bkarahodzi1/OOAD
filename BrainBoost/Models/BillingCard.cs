using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class BillingCard
    {
        [Key]
        public int BillingCardId { get; set;  }

        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVV { get; set; }

        public BillingCard() { }
    }

}
