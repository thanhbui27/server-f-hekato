using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public class Orders
    {
        public Guid Uid { get; set; }
        public int OrderId { get; set; }


        public decimal total { get; set; }

        public  DateTime createAt { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

        public UserModels users { get; set; }
        
    }
}
