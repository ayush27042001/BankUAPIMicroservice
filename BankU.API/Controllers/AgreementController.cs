using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementController : ControllerBase
    {
        private readonly IAgreementService _agreementService;

        public AgreementController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        [HttpPost]
        [Route("get-agreement")]
        public async Task<IActionResult> GetAgreement([FromBody] AgreementRequest request, CancellationToken cn)
        {
            var result = await _agreementService.GetAgreementAsync(request.UserId, cn);

            return Ok(result);
        }
    }
}
