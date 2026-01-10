using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AEPSController : ControllerBase
    {

        private readonly IAepsCheckStatusService _service;
        private readonly IAepsSignupService _signupservice;
        private readonly IAepsBiometricKycService _aepsbiometricservice;
        private readonly IAepsLoginService _IAepsLoginService;
        private readonly IAepsBapService _bapservice;
        private readonly IAepsSapService _aepsSapService;
        public AEPSController(IAepsCheckStatusService service, IAepsSignupService signupservice, IAepsBiometricKycService aepsbiometricservice, IAepsLoginService iAepsLoginService, IAepsBapService bapservice, IAepsSapService aepsSapService)
        {
            _service = service;
            _signupservice = signupservice;
            _aepsbiometricservice = aepsbiometricservice;
            _IAepsLoginService = iAepsLoginService;
            _bapservice = bapservice;
            _aepsSapService = aepsSapService;
        }

        [HttpPost("checkstatus")]
        public async Task<IActionResult> CheckStatus([FromBody] AepsCheckStatusRequestDto request,CancellationToken ct)
        {
            var result = await _service.ExecuteAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] AepsSignupRequestDto request, CancellationToken ct)
        {
            var result = await _signupservice.ExecuteAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("signupvalidate")]
        public async Task<IActionResult> SignupValidate([FromBody] AepsSignupValidateRequestDto request, CancellationToken ct)
        {
            var result = await _signupservice.SignUpValidate(request, ct);
            return Ok(result);
        }

        [HttpPost("CheckEKYCStatus")]
        public async Task<IActionResult> CheckEKYCStatus([FromBody] AepsBiometricKycStatusRequest request, CancellationToken ct)
        {
            var result = await _aepsbiometricservice.CheckEKYCStatus(request, ct);
            return Ok(result);
        }

        [HttpPost("biometrickyc")]
        public async Task<IActionResult> BiometricKyc([FromBody] AepsBiometricKycRequest request, CancellationToken ct)
        {
            var result = await _aepsbiometricservice.ExecuteAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AepsLoginRequest request, CancellationToken ct)
        {
            var result = await _IAepsLoginService.ExecuteAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("balanceenquiry")]
        public async Task<IActionResult> BalanceEnquiry([FromBody] AepsBapRequest request, CancellationToken ct)
        {
            var result = await _bapservice.ExecuteAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("ministatement")]
        public async Task<IActionResult> MiniStatement([FromBody] AepsSapApiRequest request, CancellationToken ct)
        {
            var result = await _aepsSapService.ExecuteAsync(request, ct);
            return Ok(result);
        }


    }
}
