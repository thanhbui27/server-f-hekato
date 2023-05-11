using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Repositories.Categorys
{
    public class CategoryRepositories : ICategoryRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepositories(EFDbContext context, IMapper mapper) { 
            _context= context;
            _mapper= mapper;
        }
        public async Task<ApiResult<bool>> Create(CategoryCreate cate)
        {
            var cateData = _context.categories.Where(b => b.CategoryName == cate.CategoryName)
                    .FirstOrDefault();
            if (cateData == null)
            {
                var category = _mapper.Map<Category>(cate);
                 _context.categories.Add(category);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    Message = "Thêm category thành công",
                    IsSuccessed= true,
                };
            }
            return new ApiErrorResult<bool>
            {
                IsSuccessed= false,
                Message = "Không thể thêm category"
            };
        }

        public async Task<ApiResult<List<CategoryGetAll>>> GetAll()
        {
            var categoris = await _context.categories.ToListAsync();

            var t = _mapper.Map<List<CategoryGetAll>>(categoris);

            return new ApiSuccessResult<List<CategoryGetAll>>(t);
        }

        public async Task<ApiResult<bool>> remove(CategoryRemove cate)
        {
            var  category = _mapper.Map<Category>(cate);
            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>
            {
                IsSuccessed = true,
                Message = "xoá category thành công"
            };
        }

        public async Task<ApiResult<bool>> Update(CategoryUpdate cate)
        {
            var category = await _context.categories.FirstOrDefaultAsync(x => x.CategoryId == cate.CategoryId);
            if (category != null) {
                category.CategoryName = cate.CategoryName;
                _context.categories.Update(category);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Cập nhật category thành công"
                };
            }
            return new ApiErrorResult<bool>
            {
                IsSuccessed = false,
                Message = "Cập nhật sản phẩm thất bại"
            };
        }
    }
}
