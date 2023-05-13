using DoAn.Helpers.Pagination;

namespace DoAn.ViewModels.Orders
{
    public class GetAllOrder : PagingRequestBase
    {
        public string? q { get; set; }
    }
}
