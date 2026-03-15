using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("get-invoices")]
        public async Task<IActionResult> GetInvoices([FromBody] InvoiceRequest request, CancellationToken cn)
        {
            var result = await _invoiceService.GetInvoicesAsync(request.UserId, cn);
            return Ok(result);
        }
    }
}
