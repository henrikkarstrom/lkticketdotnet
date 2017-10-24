using System.ComponentModel.DataAnnotations;

namespace LKTicket.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
