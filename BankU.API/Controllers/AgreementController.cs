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
        private readonly IAgreementSignService _signService;

        public AgreementController(
            IAgreementService agreementService,
            IAgreementSignService signService)
        {
            _agreementService = agreementService;
            _signService = signService;
        }

        [HttpPost]
        [Route("get-agreement")]
        public async Task<IActionResult> GetAgreement([FromBody] AgreementRequest request, CancellationToken cn)
        {
            var result = await _agreementService.GetAgreementAsync(request.UserId, cn);

            return Ok(result);
        }
        // SEND OTP
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] AadhaarRequest req, CancellationToken cn)
        {
            var result = await _signService.SendAadhaarOtpAsync(req, cn);
            return Ok(result);
        }

        // VERIFY + SIGN
        [HttpPost("verify-sign")]
        public async Task<IActionResult> VerifySign([FromBody] VerifyOtpRequest req, CancellationToken cn)
        {
            var result = await _signService.VerifyAndSignAsync(req, cn);
            return Ok(result);
        }
    }
}
