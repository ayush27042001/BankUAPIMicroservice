using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.ZohoMailService;
using BankUAPI.SharedKernel.ENUM;
using BankUAPI.SharedKernel.Request.ZohoMailSent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailOtpService _otp;
        private readonly IEmailService _email;
        private readonly IUserRepository _user;
        private readonly IRiskService _risk;

        public EmailController(IEmailOtpService otp, IEmailService email, IUserRepository user, IRiskService risk)
        {
            _otp = otp;
            _email = email;
            _user = user;
            _risk = risk;
        }

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendOtp(EmailRequest req)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var user = await _user.GetByEmail(req.Email);
            if (user == null)
                return Ok(new { success = true });

            if (user.IsEmailVerified == true)
                return Ok(new { success = false, message = "Already verified" });

            if (!await _otp.CanSendOtp(req.Email))
                return Ok(new { success = false, message = "Too many requests" });

            var risk = await _risk.EvaluateRisk(req.Email, req.DeviceId, ip);

            if (risk.Decision == "Block")
                return Ok(new { success = false, message = "Suspicious activity" });

            var otp = await _otp.GenerateAndSaveOtp(req.Email, req.DeviceId);

            await _email.SendOtpEmail(req.Email, otp);

            return Ok(new { success = true, message = "OTP sent" });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyEmailOtpRequest req)
        {
            var result = await _otp.VerifyOtp(req.Email, req.Otp, req.DeviceId);

            if (result != OtpVerifyStatus.Success)
            {
                return Ok(new
                {
                    success = false,
                    message = result.ToString()
                });
            }

            await _user.MarkEmailVerified(req.Email);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _risk.SaveTrustedDevice(req.Email, req.DeviceId, ip);

            return Ok(new { success = true, message = "Verified" });
        }
    }
}
