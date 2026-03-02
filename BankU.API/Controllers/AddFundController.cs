using BankUAPI.Application.Implementation.AddFund;
using BankUAPI.Application.Interface.AddFund;
using BankUAPI.SharedKernel.Request.AddFund;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddFundController : ControllerBase
    {
        private readonly IAddFundService _service; // ✅ FIX

        public AddFundController(IAddFundService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddFund([FromBody] AddFundRequest request)
        {
            var result = await _service.Process(request);
            return Ok(result);
        }
    }
}
