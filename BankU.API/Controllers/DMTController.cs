using BankUAPI.Application.Factory;
using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DMTController : ControllerBase
    {
        private readonly DmtProviderFactory _factory;
        public DMTController(DmtProviderFactory factory)
        {
            _factory = factory;
        }

        [HttpPost("remitter-profile")]
        public async Task<IActionResult> GetProfile([FromBody] RemitterProfileRequest request, CancellationToken ct)
        {
            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "0.0.0.0";
            var provider = _factory.Get(request.provider);
            return Ok(await provider.GetRemitterProfileAsync(request, ct));
        }

    }
}
