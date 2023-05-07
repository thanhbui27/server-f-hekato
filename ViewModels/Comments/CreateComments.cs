namespace DoAn.ViewModels.Comments
{
    public class CreateComments
    {
        public int Rate { get; set; }

        public string description { get; set; }

        public int ProductId { get; set; }

        public Guid Uid { get; set; }

        public DateTime? createAt { get; set; } = DateTime.Now;
    }
}
