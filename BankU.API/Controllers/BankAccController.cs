using BankUAPI.Application.Interface.BankAccount;
using BankUAPI.SharedKernel.Request.Bank_Account;
using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IBankDetails _bankDetails;
        public BankAccountController(IBankAccountService bankAccountService, IBankDetails bankDetails)
        {
            _bankAccountService = bankAccountService;
            _bankDetails = bankDetails;

        }

        [HttpPost]
        [Route("add-bank")]
        public async Task<IActionResult> AddBankAccount([FromBody] AddBankAccountRequest request, CancellationToken cn)
        {
            var result = await _bankAccountService.AddBankAccountAsync(request, cn);
            return Ok(result);
        }
        [HttpPost]
        [Route("get-bank")]
        public async Task<IActionResult> GetUserBanksAsync([FromBody] GetBankRequest request, CancellationToken cn) 
        {
            var result = await _bankDetails.GetUserBanksAsync(request, cn);
            return Ok(result);
        }
    }
}
