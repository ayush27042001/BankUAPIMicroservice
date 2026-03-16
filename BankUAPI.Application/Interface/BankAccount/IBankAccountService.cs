using BankUAPI.SharedKernel.Request.Bank_Account;
using BankUAPI.SharedKernel.Response.Bank_Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.BankAccount
{
    public interface IBankAccountService
    {
        Task<BankAccountResponse> AddBankAccountAsync(AddBankAccountRequest request, CancellationToken cn);
    }
}
