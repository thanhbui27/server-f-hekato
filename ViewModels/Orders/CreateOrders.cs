using DoAn.Models;
using DoAn.ViewModels.Product;
using DoAn.ViewModels.Users;

namespace DoAn.ViewModels.Orders
{
    public class CreateOrders
    {
        public BasicInfoUser users { get; set; }

        //public Guid Uid { get; set; }

        public List<ProductGetById> ProductIds { get; set; }

        public decimal total { get; set; }

    }
}
