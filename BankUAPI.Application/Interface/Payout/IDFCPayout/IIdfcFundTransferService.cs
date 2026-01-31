using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IIdfcFundTransferService
    {
        Task<IDFCApiResponse<FundTransferResponse>> TransferAsync(FundTransferRequest request, Registration user, string idempotencyKey, CancellationToken ct);
    }
}
