using DoAn.Helpers.ApiResponse;

namespace DoAn.Repositories.Email
{
    public interface IEmailRepositories
    {
        Task<ApiResult<string>> SendEmail(string emailPeople, string subject ,string content);
    }
}
