using DoAn.ViewModels.Users;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;
using DoAn.Helpers.Pagination;

namespace DoAn.Repositories.Users
{
    public interface IUserRepositories
    {
        Task<ApiResult<string>> Login(UserLogin user);
        Task<ApiResult<bool>> Register(UserRegister user);

        Task<ApiResult<UserModels>> GetMe();

        Task<ApiResult<bool>> Delete(string id);

        Task<PagedResult<UserModels>> getAllUser(GetAllUser user);

        Task<ApiResult<bool>> LockUser(string id, DateTime? endDate);

        Task<ApiResult<bool>> UnLockUser(string id);

        Task<ApiResult<bool>> decentralization(string id, string type);

        void createSession(Guid id);

        string CreateToken(string email, string id, string fullName, string userName, string type);
    }
}
