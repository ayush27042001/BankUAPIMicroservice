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

        private readonly IGetTicketService _getTicketService;

        public TicketController(ITicketService ticketService, IGetTicketService getTicketService)
        {
            _ticketService = ticketService;
            _getTicketService = getTicketService;
        }

        [HttpPost]
        [Route("create-ticket")]
        public async Task<IActionResult> CreateTicket([FromForm] TicketModel model, IFormFile file, CancellationToken cn)
        {
            var result = await _ticketService.CreateTicketAsync(model, file, cn);

            return Ok(result);
        }
        [HttpPost]
        [Route("get-ticket")]
        public async Task<IActionResult> GetTickets(
        [FromBody] GetTicketRequest request, CancellationToken cn)
        {
            var result = await _getTicketService.GetTicketsAsync(request.UserId, cn);
            return Ok(result);
        }
    }
}
