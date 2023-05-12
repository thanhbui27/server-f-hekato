using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn.Models
{
    public class Payment
    {
        public int paymentId { get; set; }

        [ForeignKey("Orders")]
        public int orderId { get; set; }

        public int amount { get; set; }

        public string provider { get; set; }
        
        public string status { get; set; }

        public DateTime? createAt { get; set; }

        public Orders orders { get; set; }
    }
}
