using BankUAPI.Application.Interface.Payment_Gateway.PayU;
using BankUAPI.SharedKernel.Request.Payment_Gateway.PayU;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/paymentsgateway")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPayUPaymentService _service;

        public PaymentGatewayController(IPayUPaymentService service)
        {
            _service = service;
        }

        [HttpPost("pay1")]
        public async Task<IActionResult> Pay(PaymentRequest request)
        {
            var result = await _service.ProcessPaymentAsync(request);
            return Ok(result);
        }

        [HttpGet("verify1/{txnid}")]
        public async Task<IActionResult> Verify(string txnid)
        {
            var result = await _service.VerifyPaymentAsync(txnid);
            return Ok(result);
        }
    }
}
