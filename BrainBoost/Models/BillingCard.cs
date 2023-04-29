namespace BrainBoost.Models
{
    public class BillingCard
    {
        public string Id { get; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVV { get; set; }
    }

}
