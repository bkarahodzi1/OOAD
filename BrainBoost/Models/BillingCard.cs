using System.ComponentModel.DataAnnotations;

namespace BrainBoost.Models
{
    public class BillingCard
    {
        [Key]
        public int BillingCardId { get; set;  }

        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Display(Name = "Expiry month")]
        public int ExpiryMonth { get; set; }

        [Display(Name = "Expiry year")]
        public int ExpiryYear { get; set; }

        [Display(Name = "CVV")]
        public int CVV { get; set; }

        public BillingCard() { }
    }

}
