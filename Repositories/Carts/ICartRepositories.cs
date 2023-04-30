using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Cart;

namespace DoAn.Repositories.Carts
{
    public interface ICartRepositories
    {
        Task<ApiResult<List<GetProductToCart>>> getProduct(int uid);
        Task<ApiResult<bool>> create(AddToCart cart);
        Task<ApiResult<bool>> subItem(AddToCart cart);
        Task<ApiResult<bool>> remove(RemoveItemToCart remove);

        Task<ApiResult<bool>> clearCart(int id);
    }
}
