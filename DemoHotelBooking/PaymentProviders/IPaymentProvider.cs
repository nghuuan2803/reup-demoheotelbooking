namespace DemoHotelBooking.PaymentProviders
{
    public interface IPaymentProvider
    {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request);
        PaymentResult ExecutePayment(IQueryCollection collection);
    }
}
