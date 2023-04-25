namespace DoAn.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string url_image { get; set; }

        public DateTime timeAdd { get; set; }
        public int ProductId { get; set; }

        public Product product { get; set; }
    }
}
