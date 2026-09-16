using System.Net.Sockets;

namespace AI.CustomerSupport.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public List<Ticket> Tickets { get; set; } = new();
    }
}
