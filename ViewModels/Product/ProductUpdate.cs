﻿namespace DoAn.ViewModels.Product
{
    public class ProductUpdate
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public IFormFile? Image_Url { get; set; }

        public int? quantity { get; set; }
        public decimal? PriceNew { get; set; }
        public decimal? PriceOld { get; set; }

        public string? ShortDetails { get; set; }
        public string? ProductDescription { get; set; }
        public List<int>? CategoryId { get; set; } = new List<int>();
    }
}
