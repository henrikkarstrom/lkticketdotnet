using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class TicketResponse : Ticket
    {
        [Column("rate_name")]
        public string RateName { get; set; }
        [Column("performance_id")]
        public int PerformanceId { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("category_name")]
        public string CategoryName { get; set; }
        public DateTime Start { get; set; }
        [Column("show_id")]
        public int ShowId { get; set;}
        [Column("show_name")]
        public string ShowName { get; set; }
        public bool Active { get; set; }
    }
}
