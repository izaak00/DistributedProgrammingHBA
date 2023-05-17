namespace Payment.Models
{
    public class PaymentDetails
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string OrderId { get; set; }
        public string Currency { get; set; }
        public string Address { get; set; }
    }
}
