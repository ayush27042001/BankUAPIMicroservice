using BankUAPI.Application.Interface.UserRegistration;
using BankUAPI.SharedKernel.Request;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _service;

        public UserRegistrationController(IUserRegistrationService service)
        {
            _service = service;
        }

        [HttpPost("step1")]
        public async Task<IActionResult> Step1(Step1Request req, CancellationToken ct)
        {
            return Ok(await _service.Step1(req, ct));
        }

        [HttpPost("step2")]
        public async Task<IActionResult> Step2(Step2Request req, CancellationToken ct)
        {
            return Ok(await _service.Step2(req, ct));
        }

        [HttpPost("step3")]
        public async Task<IActionResult> Step3(Step3Request req, CancellationToken ct)
        {
            return Ok(await _service.Step3(req, ct));
        }

        [HttpPost("step4")]
        public async Task<IActionResult> Step4(Step4Request req, CancellationToken ct)
        {
            return Ok(await _service.Step4(req, ct));
        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete(Step5Request req, CancellationToken ct)
        {
            return Ok(await _service.Complete(req, ct));
        }
        [HttpPost("send-aadhaar-otp")]
        public async Task<IActionResult> SendAadhaarOtp(AadhaarOtpRequest req, CancellationToken ct)
        {
            return Ok(await _service.SendAadhaarOtp(req, ct));
        }
        [HttpPost("registration-status")]
        public async Task<IActionResult> GetRegistrationStatus(RegistrationStatusRequest req, CancellationToken ct)
        {
            return Ok(await _service.GetRegistrationStatus(req, ct));
        }
    }
}
