using DemoHotelBooking.Models.Order;

namespace DemoHotelBooking.PaymentProviders.Momo
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model,string action);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
