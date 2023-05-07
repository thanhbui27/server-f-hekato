namespace DoAn.Models
{
    public class CommentsProducts
    {
        public int CommentsId { get; set; }

        public int Rate { get; set; }

        public string description { get; set; }

        public int ProductId { get; set; }

        public Guid Uid { get; set; }

        public DateTime createAt { get; set; }

        public Product product { get; set; }

        public UserModels user { get; set; }
    }
}
