using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class PerformanceResponse
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        [Column("show_id")]
        public int ShowId { get; set; }
    }
}
