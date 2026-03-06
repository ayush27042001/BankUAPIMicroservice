using BankUAPI.Application.Implementation.AddFund;
using BankUAPI.Application.Interface.AddFund;
using BankUAPI.SharedKernel.Request.AddFund;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class AddFundStatusController : ControllerBase
    {
        private readonly IAddFundStatusService _service; 

        public AddFundStatusController(IAddFundStatusService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CheckStatus([FromBody] StatusCheckRequest request, CancellationToken ct)
        {
            var result = await _service.CheckStatus(request, ct);
            return Ok(result);
        }
    }
}
