using System.ComponentModel.DataAnnotations.Schema;

namespace LKTicket.Models
{
    [Table("Rates")]
    public class Rate
    {
        public string Name { get; set; }
    }
}
