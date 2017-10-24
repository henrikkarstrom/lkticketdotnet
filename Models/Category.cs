using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    [Table("Categories")]
    public class Category
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 100000)]
        public int TicketCount { get; set; }
    }
}
