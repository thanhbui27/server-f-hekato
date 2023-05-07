using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.ViewModels.Comments;

namespace DoAn.Repositories.Comment
{
    public interface ICommentsRepositories
    {
        Task<ApiResult<List<CommentsProducts>>> getComment(int id); 
        Task<ApiResult<bool>> create(CreateComments comment);

        Task<ApiResult<bool>> delete(int id);
    }
}
