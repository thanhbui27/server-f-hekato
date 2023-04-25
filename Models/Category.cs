using System.Collections.ObjectModel;

namespace DoAn.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public Collection<ProductInCategory> GetsProductInCategories { get; set; }
    }
}
