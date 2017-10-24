using System;

namespace LKTicket.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public string Identifier { get; set; }
    }
}
