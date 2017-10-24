using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class PriceWithNameResponse
    {
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("rate_id")]
        public int RateId { get; set; }
        [Column("category_name")]
        public string CategoryName { get; set; }
        [Column("rate_name")]
        public string RateName { get; set; }
        [Column("price")]
        public int Price { get; set; }
    }
}
