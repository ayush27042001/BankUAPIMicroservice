using BankUAPI.Application.Interface.ZohoMailService;
using BankUAPI.SharedKernel.Request.ZohoMailSent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(EmailRequest request)
        {
            await _emailService.SendOtpEmail(request);
            return Ok("OTP Email Sent");
        }
    }
}
