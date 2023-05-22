using DoAn.Helpers.ApiResponse;
using DoAn.ViewModels.Orders;
using DoAn.ViewModels.VNPAY;

namespace DoAn.Repositories.MoMo
{
    public interface IMoMoRepositories
    {
        Task<ApiResult<string>> CreatePaymentUrl(CreateOrders modal, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
