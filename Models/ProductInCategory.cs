namespace DoAn.Models
{
    public class ProductInCategory
    {
        
        public int ProductId { get; set; }

        public Product GetProducts { get; set; }

        public int CategoryId { get; set; }
        public Category GetCategory { get; set; }
    }
}
