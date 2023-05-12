using DoAn.Models;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.Users;

namespace DoAn.ViewModels.Orders
{
    public class CreateOrders
    {
        public BasicInfoUser users { get; set; }

        public string typePay { get; set; }
        //public Guid Uid { get; set; }

        public List<ProductOrder> ProductIds { get; set; }

        public decimal total { get; set; }

    }

    public class ProductOrder
    {
        public int productId { get; set; }
        public int quantity { get; set; }
    }
}
