using BankUAPI.Application.Interface.Ticket;
using BankUAPI.SharedKernel.Request.Ticket;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        [Route("create-ticket")]
        public async Task<IActionResult> CreateTicket(
            [FromForm] TicketModel model,
            IFormFile file,
            CancellationToken cn)
        {
            var result = await _ticketService.CreateTicketAsync(model, file, cn);

            return Ok(result);
        }
    }
}
