using DoAn.Models;
using DoAn.ViewModels.Category;
using DoAn.ViewModels.ProductImage;

namespace DoAn.ViewModels.Product
{
    public class ProductGetById
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public List<GetProductImage> List_image { get; set; } = new List<GetProductImage>();
        public int quantity { get; set; }
        public decimal PriceNew { get; set; }
        public decimal PriceOld { get; set; }

        public string ShortDetails { get; set; }
        public string ProductDescription { get; set; }

        public DateTime? dateAdd { get; set; }
        public List<CategoryGetAll> Categories { get; set; } = new List<CategoryGetAll>();
    }
}
