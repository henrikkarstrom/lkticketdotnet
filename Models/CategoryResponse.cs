using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    public class CategoryResponse : Category
    {
        [Column("show_id")]
        public int ShowId { get; set; }
        public int Id { get; set; }
    }
}
