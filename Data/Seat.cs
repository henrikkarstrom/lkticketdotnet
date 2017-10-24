using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LKTicket.Data
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Column("performance_id")]
        public int PerformanceId { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("active_ticket_id")]
        public int ActiveTicketId { get; set; }
        [Column("profile_id")]
        public int ProfileId { get; set; }
    }
}

