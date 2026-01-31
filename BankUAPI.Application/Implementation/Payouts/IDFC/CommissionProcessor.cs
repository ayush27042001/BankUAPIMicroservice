using BankUAPI.Application.Implementation.DMT.InstantPay;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class CommissionProcessor : ICommissionProcessor
    {
        private readonly IDFCCommission _commission;

        public CommissionProcessor(IDFCCommission commission)
        {
            _commission = commission;
        }

        public async Task<IDFCApiResponse<CommissionResponse>> DebitAsync(
            decimal amount,
            Registration user)
        {
            var res = await _commission.ApplyAsync(
                amount,
                "PAYOUT",
                "IDFC",
                null,
                user.RegistrationId.ToString());

            return res.success==true
                ? IDFCApiResponse<CommissionResponse>.Ok(res)
                : IDFCApiResponse<CommissionResponse>.Fail("Wallet debit failed");
        }
    }

}
