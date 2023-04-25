﻿using AutoMapper;
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
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>();
        }

        public async Task<ApiResult<List<CategoryGetAll>>> GetAll()
        {
            var categoris = await _context.categories.ToListAsync();

            var t = _mapper.Map<List<CategoryGetAll>>(categoris);

            return new ApiSuccessResult<List<CategoryGetAll>>(t);
        }
    }
}
