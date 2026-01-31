using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IIdfcAccountStatementService
    {
        Task<AccountStatementResponse> GetMiniStatementAsync(
            AccountStatementRequest request,
            string idempotencyKey,
            string clientCode);
    }

}
