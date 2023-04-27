using DoAn.ViewModels.Users;
using DoAn.Helpers.ApiResponse;
using DoAn.Models;

namespace DoAn.Repositories.Users
{
    public interface IUserRepositories
    {
        Task<ApiResult<string>> Login(UserLogin user);
        Task<ApiResult<bool>> Register(UserRegister user);

        Task<ApiResult<UserModels>> GetMe();

        Task<ApiResult<bool>> Delete(string id);
    }
}
