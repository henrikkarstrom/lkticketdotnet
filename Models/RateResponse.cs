using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class RateResponse : Rate
    {
        [Column("show_id")]
        public int ShowId { get; set; }
        public int Id { get; set; }
    }
}
