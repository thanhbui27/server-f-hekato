namespace DoAn.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int SessionId { get; set; }
        public int ProductId { get; set; }

        public int quantity { get; set; } 

        public List<Product> product { get; set; }
        
        public Session? SessUser { get; set; }
    }
}
