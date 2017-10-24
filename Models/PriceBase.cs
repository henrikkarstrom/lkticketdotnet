using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class PriceBase {
        [Column("price")]
        public int Price { get; set; }
    }
}
