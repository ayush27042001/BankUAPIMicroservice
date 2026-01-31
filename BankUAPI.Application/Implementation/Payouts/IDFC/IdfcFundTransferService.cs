using BankUAPI.Application.Implementation.Validator;
using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Application.Interface.Validator;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class IdfcFundTransferService: IIdfcFundTransferService
    {
        private readonly IAmountValidator _amountValidator;
        private readonly IWalletValidator _walletValidator;
        private readonly ICommissionProcessor _commission;
        private readonly IIdfcBankClient _bank;
        private readonly IRefundPolicy _refund;
        private readonly IUserRepository _UserRepo;

        public IdfcFundTransferService(
            IAmountValidator amountValidator,
            IWalletValidator walletValidator,
            ICommissionProcessor commission,
            IIdfcBankClient bank,
            IRefundPolicy refund, IUserRepository UserRepo)
        {
            _amountValidator = amountValidator;
            _walletValidator = walletValidator;
            _commission = commission;
            _bank = bank;
            _refund = refund;
            _UserRepo = UserRepo;
        }

        public async Task<IDFCApiResponse<FundTransferResponse>> TransferAsync(
            FundTransferRequest request,
            Registration user,
            string idempotencyKey,
            CancellationToken ct)
        {
            user = await _UserRepo.GetUserData(request?.initiateAuthGenericFundTransferAPIReq?.UserId, ct);
            if(user==null)
            {
                return IDFCApiResponse<FundTransferResponse>.Fail("Invalid User Id");
            }
            var amt = _amountValidator.Validate(
                request.initiateAuthGenericFundTransferAPIReq.amount);

            if (amt.status == "FAILED")
                return IDFCApiResponse<FundTransferResponse>.Fail(amt.message);

            var wallet = await _walletValidator.ValidateAsync(user, amt.data!, ct);
            if (wallet.status == "FAILED")
                return IDFCApiResponse<FundTransferResponse>.Fail(wallet.message);

            var commission = await _commission.DebitAsync(amt.data, user);
            if (commission.status == "FAILED")
                return IDFCApiResponse<FundTransferResponse>.Fail(commission.message);

            var response = await _bank.TransferAsync(
                request, user, idempotencyKey, ct);

            await _refund.ApplyAsync(
                response, user, amt.data, commission.data!, ct);

            return IDFCApiResponse<FundTransferResponse>.Ok(response);
        }
    }

}
