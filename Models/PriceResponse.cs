using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class PriceResponse : PriceBase
    {
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("rate_id")]
        public int RateId { get; set; }

    }
}
