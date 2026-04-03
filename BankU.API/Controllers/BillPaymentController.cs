using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request.BillPayement;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillPaymentController : ControllerBase
    {
        private readonly IBillPaymentService _service;

        public BillPaymentController(IBillPaymentService service)
        {
            _service = service;
        }

        [HttpPost("GetBillers")]
        public async Task<IActionResult> GetBillers([FromBody] BillPaymentRequest request, CancellationToken ct)
        {
            if (request == null || request.UserId <= 0)
                return BadRequest("Invalid UserId");

            var result = await _service.GetBillers(request, ct);

            return Ok(result);
        }
        [HttpPost("GetOperators")]
        public async Task<IActionResult> GetOperators([FromBody] OperatorRequest request, CancellationToken ct)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var result = await _service.GetOperators(request, ct);

            return Ok(result);
        }
    }
}
