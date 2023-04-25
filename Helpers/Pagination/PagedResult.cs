namespace DoAn.Helpers.Pagination
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}
