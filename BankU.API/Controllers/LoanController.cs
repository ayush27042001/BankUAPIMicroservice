using BankUAPI.Application.Interface.MicroLoan;
using BankUAPI.SharedKernel.Request.MicroLoan;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("check-lead")]
        public async Task<IActionResult> CheckLead([FromBody] CheckLeadRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.CheckLeadAsync(request, cn));
        }

        [HttpPost("create-application")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateLoanRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.CreateApplicationAsync(request, cn));
        }

        [HttpPost("upload-documents")]
        public async Task<IActionResult> UploadDocuments([FromForm] UploadLoanDocsRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.UploadDocumentsAsync(request, cn));
        }

        [HttpPost("get-status")]
        public async Task<IActionResult> GetStatus([FromBody] LoanStatusRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetLoanStatusAsync(request, cn));
        }

        [HttpPost("get-terms")]
        public async Task<IActionResult> GetTerms([FromBody] LoanTermsRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetLoanTermsAsync(request, cn));
        }

        [HttpPost("get-disbursal")]
        public async Task<IActionResult> GetDisbursal([FromBody] LoanDisbursalRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetDisbursalAsync(request, cn));
        }
    }
}
