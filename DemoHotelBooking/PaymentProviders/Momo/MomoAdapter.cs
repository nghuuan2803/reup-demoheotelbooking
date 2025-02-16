using DemoHotelBooking.Models.Order;

namespace DemoHotelBooking.PaymentProviders.Momo
{
    public class MomoAdapter : IPaymentProvider
    {
        private readonly IMomoService _momoService;

        public MomoAdapter(IMomoService momoService)
        {
            _momoService = momoService;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request)
        {
            var orderInfo = new OrderInfoModel
            {
                OrderId = request.OrderId ?? DateTime.UtcNow.Ticks.ToString(),
                FullName = request.FullName,
                Amount = request.Amount,
                OrderInfo = request.OrderInfo
            };

            var momoResponse = await _momoService.CreatePaymentAsync(orderInfo, request.Action);

            return new PaymentResponse
            {
                PaymentUrl = momoResponse.PayUrl,
                IsSuccess = momoResponse.ErrorCode == 0,
                ErrorMessage = momoResponse.ErrorCode != 0 ? momoResponse.Message : null
            };
        }

        public PaymentResult ExecutePayment(IQueryCollection collection)
        {
            var result = _momoService.PaymentExecuteAsync(collection);
            return new PaymentResult
            {
                OrderId = result.OrderId,
                Amount = double.TryParse(result.Amount, out var amount) ? amount : 0,
                OrderInfo = result.OrderInfo,
                IsSuccess = result.ErrorCode == 0,
                ErrorCode = result.ErrorCode.ToString()
            };
        }
    }
}
