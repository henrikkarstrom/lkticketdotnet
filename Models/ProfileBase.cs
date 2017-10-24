using System.ComponentModel.DataAnnotations;

namespace LKTicket.Models
{
    public class ProfileBase
    {
        [Required]
        public string Name { get; set; }
    }
}
