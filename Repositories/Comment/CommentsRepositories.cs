using AutoMapper;
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
            var cm = _mapper.Map<CommentsProducts>(comment);
             _context.commentProducts.Add(cm);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>
            {
                IsSuccessed= true,
                Message = "Thêm bình luận thành công"
            };
        }

        public async Task<ApiResult<bool>> delete(int id)
        {
            _context.commentProducts.Remove(new CommentsProducts { CommentsId = id});
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>
            {
                IsSuccessed = true,
                Message = "Xoá bình luận thành công"
            };
        }

        public async Task<ApiResult<List<CommentsProducts>>> getComment(int id)
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
    }
}
