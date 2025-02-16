namespace DemoHotelBooking.PaymentProviders.VnPay
{
    public class VnPayAdapter : IPaymentProvider
    {
        private readonly IVnPayService _vnPayService;

        public VnPayAdapter(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request)
        {
            var vnPayRequest = new VnPaymentRequestModel
            {
                FullName = request.FullName,
                Amount = request.Amount,
                CreateDate = request.CreateDate,
                BookingId = request.BookingId,
                Description = request.OrderInfo
            };

            var paymentUrl = _vnPayService.CreatePaymentUrl(
                request.Context,
                vnPayRequest,
                request.Action
            );

            return new PaymentResponse
            {
                PaymentUrl = paymentUrl,
                IsSuccess = !string.IsNullOrEmpty(paymentUrl)
            };
        }

        public PaymentResult ExecutePayment(IQueryCollection collection)
        {
            var result = _vnPayService.PaymentExecute(collection);
            var amount = Convert.ToDouble(collection.FirstOrDefault(k => k.Key == "vnp_Amount").Value) / 100;

            return new PaymentResult
            {
                OrderId = result.OrderId,
                Amount = amount,
                OrderInfo = result.OrderDescription,
                IsSuccess = result.Success,
                TransactionId = result.TransactionId,
                ErrorCode = result.VnPayResponseCode
            };
        }
    }
}
