using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.Validator;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Validator
{
    public sealed class WalletValidator : IWalletValidator
    {
        private readonly ICommonRepositry _repo;
        public WalletValidator(ICommonRepositry repo)
        {
            _repo = repo;
        }

        public async Task<IDFCApiResponse<bool>> ValidateAsync(
            Registration user,
            decimal amount,
            CancellationToken ct)
        {
            var res = await _repo.WalletCheckValidationAsync(
                user.RegistrationId, amount, ct);

            return res.success
                ? IDFCApiResponse<bool>.Ok(true)
                : IDFCApiResponse<bool>.Fail(res.message);
        }
    }

}
