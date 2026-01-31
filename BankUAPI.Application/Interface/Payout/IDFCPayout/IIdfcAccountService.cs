using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IIdfcAccountService
    {
        Task<IdfcAccountBalanceResponse> GetAccountBalanceAsync(string accountNumber, string idempotencyKey, string clientCode);
    }
}
