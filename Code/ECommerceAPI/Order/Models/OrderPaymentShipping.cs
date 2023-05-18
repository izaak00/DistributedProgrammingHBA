namespace Order.Models
{
    public class OrderPaymentShipping
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public string Owner { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
        public string ShippingStatus { get; set; }
    }
}
