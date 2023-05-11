using DoAn.Helpers.Pagination;

namespace DoAn.ViewModels.Users
{
    public class GetAllUser : PagingRequestBase
    {
        public string? p { get; set; }
    }
}
