using DoAn.ViewModels.Category;

namespace DoAn.ViewModels.Product
{
    public class ProductGetAll
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Image_Url { get; set; }

        public int quantity { get; set; }
        public decimal PriceNew { get; set; }
        public decimal PriceOld { get; set; }

        public string ShortDetails { get; set; }
        public string ProductDescription { get; set; }

        public DateTime? dateAdd { get; set; }
        public List<CategoryGetAll> Categories { get; set; } = new List<CategoryGetAll>();
    }

}
