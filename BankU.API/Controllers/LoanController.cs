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
            return Ok(await _loanService.CheckLead(request, cn));
        }

        [HttpPost("create-application")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateLoanRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.CreateApplication(request, cn));
        }

        [HttpPost("upload-documents")]
        public async Task<IActionResult> UploadDocuments([FromForm] UploadLoanDocsRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.UploadDocuments(request, cn));
        }

        [HttpPost("get-status")]
        public async Task<IActionResult> GetStatus([FromBody] LoanStatusRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetLoanStatus(request, cn));
        }

        [HttpPost("get-terms")]
        public async Task<IActionResult> GetTerms([FromBody] LoanTermsRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetLoanTerms(request, cn));
        }

        [HttpPost("get-disbursal")]
        public async Task<IActionResult> GetDisbursal([FromBody] LoanDisbursalRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetDisbursal(request, cn));
        }
        [HttpPost("reupload-status")]
        public async Task<IActionResult> GetApplicationDetails( [FromBody] LoanStatusRequest request,CancellationToken cn)
        {
            return Ok(await _loanService.GetApplicationDetails(request, cn));
        }

        [HttpPost("reupload-document")]
        public async Task<IActionResult> ReUploadDocument( [FromForm] ReUploadLoanDocRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.ReUploadDocument(request, cn));
        }
        [HttpPost("applications-list")]
        public async Task<IActionResult> GetApplicationsByUser( [FromBody] LoanApplicationsByUserRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.GetApplicationsByUser(request, cn));
        }
        [HttpPost("update-application")]
        public async Task<IActionResult> UpdateLoanApplication( [FromBody] UpdateLoanApplicationRequest request, CancellationToken cn)
        {
            return Ok(await _loanService.UpdateLoanApplication(request, cn));
        }
    }
}
