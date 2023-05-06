using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int quantity { get; set; }

        public DateTime createAt { get; set; }


        public Product products { get; set; }


        public Orders orders { get; set; }
    }
}
