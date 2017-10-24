using System.ComponentModel.DataAnnotations;

namespace LKTicket.Models
{
    public class TicketRequest
    {
        [Range(0, double.MaxValue)]
        public int PerformanceId { get; set; }
        [Range(0, double.MaxValue)]
        public int RateId { get; set; }
        [Range(0, double.MaxValue)]
        public int CategoryId { get; set; }
        [Range(1, double.MaxValue)]
        public int Count { get; set; }
        [Range(0, double.MaxValue)]

        //TODO: move this properties
        public int ProfileId { get; set; }
        public int UserId { get; set; }
    }
}
