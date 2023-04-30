namespace DoAn.Models
{
    public class Session
    {
        public int SessionId { get; set; }

        public Guid Uid { get; set; }

        public UserModels user { get; set; }

        public List<Cart>? Carts { get; set; }
    }
}
