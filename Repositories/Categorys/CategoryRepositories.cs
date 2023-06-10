using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Helpers.FuncHelps;
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
            try
            {
                var func = new FuncHelp();
                
                var cateData = _context.categories.Where(b => b.CategoryName == cate.CategoryName)
                   .FirstOrDefault();
                if (cateData == null)
                {
                    var category = _mapper.Map<Category>(cate);
                    category.keyCategory = func.RemoveVietnameseTone(cate.CategoryName).Replace(" ", "_").ToLower();
                    _context.categories.Add(category);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        Message = "Thêm category thành công",
                        IsSuccessed = true,
                    };
                }
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = "Không thể thêm category"
                };
            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
           
        }

        public async Task<ApiResult<List<CategoryGetAll>>> GetAll()
        {
            try
            {
                var categoris = await _context.categories.ToListAsync();

                var t = _mapper.Map<List<CategoryGetAll>>(categoris);

                return new ApiSuccessResult<List<CategoryGetAll>>(t);

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<CategoryGetAll>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<bool>> remove(CategoryRemove cate)
        {
            try {
                var category = _mapper.Map<Category>(cate);
                _context.categories.Remove(category);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "xoá category thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<ApiResult<bool>> Update(CategoryUpdate cate)
        {
            try
            {
                var func = new FuncHelp();
                var category = await _context.categories.FirstOrDefaultAsync(x => x.CategoryId == cate.CategoryId);
                if (category != null)
                {
                    category.CategoryName = cate.CategoryName;
                    category.keyCategory = func.RemoveVietnameseTone(cate.CategoryName).Replace(" ", "_").ToLower();
               
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
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }

        }
    }
}
