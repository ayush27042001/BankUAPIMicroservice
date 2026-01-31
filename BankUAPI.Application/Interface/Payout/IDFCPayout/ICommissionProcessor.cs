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
    public interface ICommissionProcessor
    {
        Task<IDFCApiResponse<CommissionResponse>> DebitAsync(decimal amount, Registration user);
    }

}
