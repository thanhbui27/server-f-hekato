using DoAn.Helpers.ApiResponse;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.VNPAY;

namespace DoAn.Repositories.VNPAY
{
    public interface IVnpayRepositories
    {
        Task<ApiResult<string>> CreatePaymentUrl(CreateOrders model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
