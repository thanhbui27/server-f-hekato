using Microsoft.AspNetCore.Mvc;

namespace DoAn.ViewModels.Product
{
    public class ProductKeyFilter
    {
        public List<string>? categories { get; set; }
        public List<string>? productBrand { get; set; }
        public string? discount { get; set; }
        public string? rating { get; set; }
        public string? price_filter { get; set; }
    }
}
