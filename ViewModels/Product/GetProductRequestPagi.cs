using DoAn.Helpers.Pagination;

namespace DoAn.ViewModels.Product
{
    public class GetProductRequestPagi : PagingRequestBase
    {
        public string? q { get; set; }
    }
}
