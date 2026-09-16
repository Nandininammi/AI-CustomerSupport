using AI.CustomerSupport.Services;
using Microsoft.AspNetCore.Mvc;
using AI.CustomerSupport.Models;

namespace AI.CustomerSupport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicket(int ticketId)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(ticketId);

            if (ticket == null)
            {
                return NotFound(new
                {
                    message = $"Ticket {ticketId} was not found."
                });
            }

            return Ok(ticket);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicket(
         CreateTicketRequest request)
        {
            var ticket = await _ticketService.CreateTicketAsync(
                request.CustomerId,
                request.Subject,
                request.Description);

            return Ok(ticket);
        }
    }
}