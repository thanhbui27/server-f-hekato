﻿using AutoMapper;
using DoAn.EF;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Comments;

namespace DoAn.Repositories.Comment
{
    public class CommentsRepositories : ICommentsRepositories
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;

        public CommentsRepositories( EFDbContext context, IMapper mapper )
        {
            _context= context;
            _mapper= mapper;
        }
        public async Task<ApiResult<bool>> create(CreateComments comment)
        {
            try
            {
                var commentProduct = _context.commentProducts.Where(item => item.ProductId == comment.ProductId && item.Uid == comment.Uid).FirstOrDefault();
                if(commentProduct == null )
                {
                    var cm = _mapper.Map<CommentsProducts>(comment);
                    _context.commentProducts.Add(cm);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Thêm bình luận thành công"
                    };
                }else
                {
                    var cm = _mapper.Map<CommentsProducts>(comment);
                    cm.Rate = commentProduct.Rate;
                    _context.commentProducts.Add(cm);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>
                    {
                        IsSuccessed = true,
                        Message = "Thêm bình luận thành công"
                    };
                }
              

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

        public async Task<ApiResult<bool>> delete(int id)
        {
            try
            {
                _context.commentProducts.Remove(new CommentsProducts { CommentsId = id});
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>
                {
                    IsSuccessed = true,
                    Message = "Xoá bình luận thành công"
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

        public async Task<ApiResult<List<CommentsProducts>>> getComment(int id)
        {
            try
            {

               var comment = _context.commentProducts.Select(p => new CommentsProducts { 
                    CommentsId = p.CommentsId,
                    createAt = p.createAt,
                    Rate = p.Rate,
                    user = _context.Users.Where(u => u.Id == p.Uid).FirstOrDefault(),      
                    description = p.description,
                    ProductId = p.ProductId,
               }).Where(x => x.ProductId == id).OrderByDescending(x => x.createAt).ToList();

                return new ApiSuccessResult<List<CommentsProducts>>(comment);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<CommentsProducts>>
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }
    }
}
