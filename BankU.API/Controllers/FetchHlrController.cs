using BankUAPI.Application.Implementation.Recharge;
using BankUAPI.Application.Interface.Recharge;
using BankUAPI.SharedKernel.Request.Recharge;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FetchHlrController : ControllerBase
    {
        private readonly IFetchHlrService _service;

        public FetchHlrController(IFetchHlrService service)
        {
            _service = service;
        }

        [HttpPost("fetch")]
        public async Task<IActionResult> Fetch([FromBody] FetchHlrRequest request, CancellationToken ct)
        {
            var result = await _service.FetchDetails(request, ct);
            return Ok(result);
        }
    }
}
