using BankUAPI.Application.Interface.Recharge;
using BankUAPI.SharedKernel.Request.Recharge;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
 
    [ApiController]
    [Route("api/[controller]")]
    public class RechargePlanController : ControllerBase
    {
        private readonly IRechargePlanService _service;

        public RechargePlanController(IRechargePlanService service)
        {
            _service = service;
        }

        [HttpPost("plans")]
        public async Task<IActionResult> GetPlans([FromBody] RechargePlanRequest request, CancellationToken ct)
        {
            var result = await _service.GetRechargePlans(request, ct);
            return Ok(result);
        }

        [HttpPost("circles")]
        public async Task<IActionResult> GetCircles([FromBody] CircleRequest request, CancellationToken ct)
        {
            var result = await _service.GetCircles(request, ct);
            return Ok(result);
        }
    }
}

