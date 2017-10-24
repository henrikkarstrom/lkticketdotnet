using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        [Column("seat_id")]
        public int SeatId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("rate_id")]
        public int RateId { get; set; }
        public int Price { get; set; }
        public bool Paid { get; set; }
        public bool Printed { get; set; }
        public bool Scanned { get; set; }
        public bool Confirmed { get; set; }
    }
}
