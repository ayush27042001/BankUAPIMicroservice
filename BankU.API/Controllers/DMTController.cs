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
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "0.0.0.0";
            var provider = _factory.Get(request.provider);
            return Ok(await provider.GetRemitterProfileAsync(request, ct));
        }

        [HttpPost("remitter-registration")]
        public async Task<IActionResult> RemitterRegistration([FromBody] RemitterRegistrationRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "0.0.0.0";
            var provider = _factory.Get(request.provider);
            return Ok(await provider.GetRemitterRegistrationAsync(request, ct));
        }

        [HttpPost("remitter-registration-validate")]
        public async Task<IActionResult> RegistrationValidate([FromBody] RemitterRegistrationRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "0.0.0.0";
            var provider = _factory.Get(request.provider);
            return Ok(await provider.RemitterRegistrationValidateAsync(request, ct));
        }

        [HttpPost("remitter-EKYC")]
        public async Task<IActionResult> RegisterBeneficiary([FromBody] RemitterEKYC request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.RegisterEKYCAsync(request, ct);
            return Ok(result);
        }

        [HttpPost("remitter-beneficiary-registration")]
        public async Task<IActionResult> BeneficiaryRegistration([FromBody] RemitterBeneficiaryRegistration request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.BeneficiaryRegistraton(request, ct);
            return Ok(result);
        }

        [HttpPost("remitter-beneficiary-registration-validate")]
        public async Task<IActionResult> BeneficiaryRegistrationValidate([FromBody] BeneficiaryVerifyRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.BeneficiaryRegistratonVerification(request, ct);
            return Ok(result);
        }

        [HttpPost("beneficiary-delete")]
        public async Task<IActionResult> BeneficiaryDelete([FromBody] BeneficiaryDeleteRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.DeleteBeneficiary(request, ct);
            return Ok(result);
        }

        [HttpPost("beneficiary-delete-verify")]
        public async Task<IActionResult> BeneficiaryDeleteVerify([FromBody] BeneficiaryVerifyRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.DeleteBeneficiaryVerify(request, ct);
            return Ok(result);
        }

        [HttpPost("UpdateBankData")]
        public async Task<IActionResult> UpdateBankData(string UserId, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            string EndpointIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.FetchBankListAndSyncInDB(EndpointIp, UserId, ct);
            return Ok(result);
        }

        [HttpPost("FetchBankList")]
        public async Task<IActionResult> FetchBankList(CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            var provider = _factory.Get("instantpay");
            var result = await provider.FetchBankList(ct);
            return Ok(result);
        }

        [HttpPost("DMTTransactionOTP")]
        public async Task<IActionResult> DMTTransactionOTP([FromBody] GenerateTransactionOTPRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                     ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            request.ip = ip?.Split(',').First().Trim() ?? "0.0.0.0";

            var provider = _factory.Get("instantpay");
            var result = await provider.DMTTransactionOTPGenerate(request, ct);
            return Ok(result);
        }

        [HttpPost("DMTTransaction")]
        public async Task<IActionResult> DMTTransaction([FromBody] DmtTransactionRequest request, CancellationToken ct)
        {
            int uid = 0;
            string username = null;

            var userIdClaim = User?.FindFirst("userid");
            var usernameClaim = User?.FindFirst("username");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out uid) && usernameClaim != null && !string.IsNullOrWhiteSpace(usernameClaim.Value))
            {
                username = usernameClaim.Value;
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                var headerUserId = Request.Headers["userid"].FirstOrDefault();
                var headerUsername = Request.Headers["username"].FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(headerUserId) &&
                    int.TryParse(headerUserId, out uid) &&
                    !string.IsNullOrWhiteSpace(headerUsername))
                {
                    username = headerUsername;
                }
            }

            if (uid == 0 || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    message = "Invalid or missing userid/username in token and headers"
                });
            }

            request.IP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
               ?? HttpContext.Connection.RemoteIpAddress?.ToString()
               ?? "0.0.0.0";
            var provider = _factory.Get("instantpay");
            var result = await provider.DMTTransaction(request, ct);
            return Ok(result);
        }

        [HttpGet("crash")]
        public IActionResult Crash()
        {
            throw new Exception("TEST GLOBAL EXCEPTION");
        }
    }
}
