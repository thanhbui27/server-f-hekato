using DoAn.Helpers.ApiResponse;
using DoAn.ViewModels.ProductAction;

namespace DoAn.Repositories.ProductAction
{
    public interface IProductActionRepositories
    {
        Task<ApiResult<bool>> Update(UpdateProductAction update);
        Task<ApiResult<bool>> Delete(DeleteProductAction delete);

    }
}
