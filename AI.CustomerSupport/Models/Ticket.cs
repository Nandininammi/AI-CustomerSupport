namespace AI.CustomerSupport.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public int CustomerId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public Customer? Customer { get; set; }
    }
}
