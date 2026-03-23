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
        public async Task<IActionResult> Step1(Step1Request req)
        {
            return Ok(await _service.Step1Async(req));
        }

        [HttpPost("step2")]
        public async Task<IActionResult> Step2(Step2Request req)
        {
            return Ok(await _service.Step2Async(req));
        }

        [HttpPost("step3")]
        public async Task<IActionResult> Step3(Step3Request req)
        {
            return Ok(await _service.Step3Async(req));
        }

        [HttpPost("step4")]
        public async Task<IActionResult> Step4(Step4Request req)
        {
            return Ok(await _service.Step4Async(req));
        }

        [HttpPost("complete")]
        public async Task<IActionResult> Complete(Step5Request req)
        {
            return Ok(await _service.CompleteAsync(req));
        }
        [HttpPost("send-aadhaar-otp")]
        public async Task<IActionResult> SendAadhaarOtp(AadhaarOtpRequest req)
        {
            return Ok(await _service.SendAadhaarOtpAsync(req));
        }
    }
}
