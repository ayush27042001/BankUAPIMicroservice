using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetMpinController : ControllerBase
    {
        private readonly IResetMpinService _resetMpinService;

        public ResetMpinController(IResetMpinService resetMpinService)
        {
            _resetMpinService = resetMpinService;
        }

        [HttpPost]
        [Route("update-mpin")]
        public async Task<IActionResult> ResetMpin([FromBody] ResetMpin model, CancellationToken cn)
        {
            var result = await _resetMpinService.ResetMpinAsync(model, cn);

            return Ok(result);
        }
    }
}
