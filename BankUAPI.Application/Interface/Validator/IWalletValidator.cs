using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Validator
{
    public interface IWalletValidator
    {
        Task<IDFCApiResponse<bool>> ValidateAsync(
            Registration user,
            decimal amount,
            CancellationToken ct);
    }

}
