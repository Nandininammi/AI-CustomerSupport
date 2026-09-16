using AI.CustomerSupport.Data;
using AI.CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace AI.CustomerSupport.Services
{
    public class TicketService
    {
        private readonly SupportDbContext _context;

        public TicketService(SupportDbContext context)
        {
            _context = context;
        }

        public async Task<object?> GetTicketByIdAsync(int ticketId)
        {
            return await _context.Tickets
                .Include(t => t.Customer)
                .Where(t => t.TicketId == ticketId)
                .Select(t => new
                {
                    t.TicketId,
                    CustomerName = t.Customer!.Name,
                    t.Subject,
                    t.Description,
                    t.Status,
                    t.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<object> CreateTicketAsync(
            int customerId,
            string subject,
            string description)
        {
            var ticket = new Ticket
            {
                CustomerId = customerId,
                Subject = subject,
                Description = description,
                Status = "Open",
                CreatedDate = DateTime.Now
            };

            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();

            return new
            {
                ticket.TicketId,
                ticket.CustomerId,
                ticket.Subject,
                ticket.Description,
                ticket.Status,
                ticket.CreatedDate
            };
        }

        public string GetToolDescription()
        {
            return """
        Tool name: get_ticket_by_id

        Purpose:
        Get the details of a customer support ticket from the company database.

        Required input:
        ticketId - The numeric ticket ID.

        Use this tool when the customer asks about:
        - ticket status
        - ticket details
        - ticket subject
        - ticket description
        """;
        }
    }
}