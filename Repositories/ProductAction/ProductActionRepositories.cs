﻿using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.ProductAction;

namespace DoAn.Repositories.ProductAction
{
    public class ProductActionRepositories : IProductActionRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public ProductActionRepositories(EFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<ApiResult<bool>> Delete(DeleteProductAction delete)
        {
            try
            {
                var _productAction = _mapper.Map<ProductActions>(delete);
                _context.productActions.Remove(_productAction);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed= true,
                    Message = "Xoá thành công"
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


        public async Task<ApiResult<bool>> Update(UpdateProductAction update)
        {
            try
            {
                var _productAction = _context.productActions.FirstOrDefault(x => x.Id == update.Id);

                _productAction.BestSeller = update.BestSeller;
                _productAction.NewArrival = update.NewArrival;
                _productAction.Featured = update.Featured;
                _productAction.SpecialOffer = update.SpecialOffer;

                _context.productActions.Update(_productAction);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Cập nhật thành công"
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
