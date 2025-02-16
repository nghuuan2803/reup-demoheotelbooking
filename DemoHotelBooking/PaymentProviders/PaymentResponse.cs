namespace DemoHotelBooking.PaymentProviders
{
    public class PaymentResponse
    {
        public string PaymentUrl { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
