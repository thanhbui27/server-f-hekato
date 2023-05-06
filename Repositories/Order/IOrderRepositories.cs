using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Orders;

namespace DoAn.Repositories.Order
{
    public interface IOrderRepositories
    {
        Task<ApiResult<bool>> create(CreateOrders create);

        Task<ApiResult<List<Orders>>> getOrderByUser(Guid Uid);

        Task<ApiResult<bool>> remove(int OrderId);
    }
}
