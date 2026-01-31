using BankUAPI.Application.Implementation.Payouts.IDFC;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IDFCPayoutController : ControllerBase
    {
        private readonly IIdfcAccountService _service;
        private readonly IIdfcBeneValidationService _beneservice;
        private readonly IIdfcPaymentStatusService _statusService;
        private readonly IIdfcAccountStatementService _statementService;
        private readonly IIdfcFundTransferService _fundTransferService;
        public IDFCPayoutController(IIdfcAccountService service, IIdfcBeneValidationService beneservice, IIdfcPaymentStatusService statusService, IIdfcAccountStatementService statementService, IIdfcFundTransferService fundTransferService)
        {
            _service = service;
            _beneservice = beneservice;
            _statusService = statusService;
            _statementService = statementService;
            _fundTransferService = fundTransferService;
        }

        [HttpGet("balance/{accountNumber}/{CompanyCode}")]
        public async Task<IActionResult> GetBalance(string accountNumber, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey, string? CompanyCode)
        {
            idempotencyKey ??= Guid.NewGuid().ToString("N");
            var result = await _service.GetAccountBalanceAsync(accountNumber, idempotencyKey, CompanyCode);
            return Ok(result);
        }

        [HttpPost("idfc/bene-validation")]
        public async Task<IActionResult> BeneValidate(BeneValidationRequest request, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            if(string.IsNullOrEmpty(request?.beneValidationReq?.CompanyCode))
            {
                return Ok("Please Provide valid Company Code!");
            }
            idempotencyKey ??= Guid.NewGuid().ToString("N");
            var result = await _beneservice.ValidateAsync(
                request,
                idempotencyKey,
                request?.beneValidationReq?.CompanyCode);

            return Ok(result);
        }

        [HttpPost("idfc/payment-status")]
        public async Task<IActionResult> PaymentStatus(PaymentStatusEnquiryRequest request, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            if (string.IsNullOrEmpty(request?.paymentTransactionStatusReq?.CompanyCode))
            {
                return Ok("Please Provide valid Company Code!");
            }
            idempotencyKey ??= Guid.NewGuid().ToString("N");
            var result = await _statusService.EnquireAsync(
                request,
                idempotencyKey,
                request?.paymentTransactionStatusReq?.CompanyCode);

            return Ok(result);
        }

        [HttpPost("idfc/mini-statement")]
        public async Task<IActionResult> MiniStatement(AccountStatementRequest request, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
        {
            if (string.IsNullOrEmpty(request?.getAccountStatementReq?.companyCode))
            {
                return Ok("Please Provide valid Company Code!");
            }
            idempotencyKey ??= Guid.NewGuid().ToString("N");
            var result = await _statementService
                .GetMiniStatementAsync(request, idempotencyKey, request?.getAccountStatementReq?.companyCode);

            return Ok(result);
        }

        [HttpPost("idfc/fund-transfer")]
        public async Task<IActionResult> FundTransfer([FromBody] FundTransferRequest request, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey, CancellationToken ct)
        {
            if (request?.initiateAuthGenericFundTransferAPIReq == null)
            {
                return Ok(new IDFCApiResponse<FundTransferResponse>
                {
                    status = "ERR",
                    message = "Invalid request payload"
                });
            }
            idempotencyKey ??= Guid.NewGuid().ToString("N");

            if (string.IsNullOrEmpty(request?.initiateAuthGenericFundTransferAPIReq?.companyCode))
            {
                return Ok(new IDFCApiResponse<FundTransferResponse>
                {
                    status = "ERR",
                    message = "Please Provide Company Code"
                });
            }

            var result = await _fundTransferService.TransferAsync(
                request,
                null,
                idempotencyKey!,
                ct);
            return Ok(result);
        }


    }
}
