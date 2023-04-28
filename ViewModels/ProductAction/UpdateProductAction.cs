namespace DoAn.ViewModels.ProductAction
{
    public class UpdateProductAction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public bool NewArrival { get; set; }

        public bool BestSeller { get; set; }

        public bool Featured { get; set; }

        public bool SpecialOffer { get; set; }

    }
}
