using DoAn.ViewModels.Users;
using Learn.ViewModels.common;

namespace DoAn.Repositories.Users
{
    public interface IUserRepositories
    {
        Task<ApiResult<string>> Login(UserLogin user);
        Task<ApiResult<bool>> Register(UserRegister user);
    }
}
