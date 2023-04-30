
using DoAn.ViewModels.Category;
using DoAn.ViewModels.Product;

namespace DoAn.ViewModels.Cart
{
    public class GetProductToCart
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public ProductGetAll productGetAll { get; set; }
        public int quantity { get; set; }


    }
}
