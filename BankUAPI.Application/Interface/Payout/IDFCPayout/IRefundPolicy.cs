using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IRefundPolicy
    {
        Task ApplyAsync(FundTransferResponse response, Registration user, decimal amount, CommissionResponse commission, CancellationToken ct);
    }
}
