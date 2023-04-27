using DoAn.ViewModels.Category;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;

namespace DoAn.Repositories.Categorys
{
    public interface ICategoryRepositories
    {
        Task<ApiResult<List<CategoryGetAll>>> GetAll();
        Task<ApiResult<bool>> Create(CategoryCreate cate);
        Task<ApiResult<bool>> Update(CategoryUpdate cate);
        Task<ApiResult<bool>> remove(CategoryRemove cate);
    }
}
