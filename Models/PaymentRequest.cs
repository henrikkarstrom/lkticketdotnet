using System.ComponentModel.DataAnnotations;

namespace LKTicket.Models
{
    public class PaymentRequest
    {
        public int Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }

        public int UserId { get; set; }
        public int ProfileId { get; set; }
        [Required]
        public string PaymentReference { get; internal set; }
    }
}
