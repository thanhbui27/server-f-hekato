using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.Pagination;
using DoAn.Models;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.ProductImage;

namespace DoAn.Repositories.Products
{
    public interface IProductRepositories
    {
        Task<PagedResult<GetProductByPa>> GetAll(GetProductRequestPagi request);

        Task<ApiResult<List<Product>>> SearchProduct(string key);

        Task<ApiResult<bool>> create(ProductCreate create);

        Task<ApiResult<bool>> update(ProductUpdate update);

        Task<ApiResult<bool>> delete(ProductDelete delete);

        Task<ApiResult<ProductGetById>> GetById(int id);

        Task<ApiResult<bool>> RemoveCategory(ProductRemoveCatgory rm);

        Task<ApiResult<List<GetProductImage>>> UploadImage(int id, List<IFormFile> files);

        Task<ApiResult<List<GetProductImage>>> GetAllImageById(int id);
        
        Task<ApiResult<List<GetProductByPa>>> GetProductFeature();
        Task<ApiResult<List<GetProductByPa>>> GetProductBestSeller();
        Task<ApiResult<List<GetProductByPa>>> GetProductSpecialOffer();
        Task<ApiResult<List<GetProductByPa>>> GetProductNewArrival();
        Task<ApiResult<List<GetProductByPa>>> GetProductTrending();
        Task<ApiResult<List<GetProductByPa>>> GetProductTrendSmall();
    }
}
