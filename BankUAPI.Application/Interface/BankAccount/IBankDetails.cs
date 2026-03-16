using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.BankAccount
{
    public interface IBankDetails
    {
        Task<BankDetailsResponse> GetUserBanksAsync(GetBankRequest request, CancellationToken cn);
    }
}
