namespace AI.CustomerSupport.Models
{
    public class CreateTicketRequest
    {
        public int CustomerId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}