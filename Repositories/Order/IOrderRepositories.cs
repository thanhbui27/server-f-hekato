using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.Pagination;
using DoAn.Models;
using DoAn.ViewModels.Orders;

namespace DoAn.Repositories.Order
{
    public interface IOrderRepositories
    {
        Task<ApiResult<bool>> create(CreateOrders create);

        Task<ApiResult<List<Orders>>> getOrderByUser(Guid Uid);
        Task<PagedResult<Orders>> getAllOrders(GetAllOrder getall);

        Task<ApiResult<bool>> updateStatusOrder(int id, string status);
        Task<ApiResult<bool>> remove(int OrderId);

        Task<ApiResult<Orders>> getDetailtOrder(int id);

    }
}
