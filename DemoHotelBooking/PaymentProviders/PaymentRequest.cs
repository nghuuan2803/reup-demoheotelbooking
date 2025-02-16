namespace DemoHotelBooking.PaymentProviders
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public string FullName { get; set; }
        public string OrderInfo { get; set; }
        public double Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public int BookingId { get; set; }
        public string Action { get; set; }
        public HttpContext Context { get; set; }
    }
}
