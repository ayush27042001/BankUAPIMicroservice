using BankUAPI.Application.Interface;
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
    public sealed class IdfcRefundPolicy : IRefundPolicy
    {
        private readonly ICommonRepositry _repo;

        public IdfcRefundPolicy(ICommonRepositry repo)
        {
            _repo = repo;
        }

        public async Task ApplyAsync(FundTransferResponse response, Registration user, decimal amount, CommissionResponse commission, CancellationToken ct)
        {
            var status = response.initiateAuthGenericFundTransferAPIResp.metaData.status ?? "";
            

            if (status is not ("FAILED" or "REJECTED" or "ERROR"))
                return;

            await _repo.RefundWalletBalance(
                user.RegistrationId,
                Convert.ToDecimal(amount + commission.RetailerCommissionCharge),
                "PAYOUT_REFUND",
                ct);
        }
    }

}
