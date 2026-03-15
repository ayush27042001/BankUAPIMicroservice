using BankUAPI.Application.Interface.KYC;
using BankUAPI.SharedKernel.Request.KYC;
using BankUAPI.SharedKernel.Response.KYC;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly IAddKycService _kycService;
        private readonly IGetKycService _getkycService;
        public KycController(IAddKycService kycService, IGetKycService getKycService)
        {
            _kycService = kycService;
            _getkycService = getKycService;
        }

        [HttpPost]
        [Route("upload-kyc")]
        public async Task<IActionResult> UploadKyc([FromForm] AddKycRequest request, CancellationToken cn)
        {
            var result = await _kycService.UploadKycAsync(request, cn);

            return Ok(result);
        }
        [HttpPost]
        [Route("get-kyc")]
        public async Task<IActionResult> GetKyc([FromBody] GetKycRequest request, CancellationToken cn)
        {
            var result = await _getkycService.GetKycAsync(request.RegistrationId, cn);

            return Ok(result);
        }

    }
}
