using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IIDFCCommissionService
    {
        Task<CommissionResponse> ApplyAsync(decimal txnAmount, string serviceId, string providerId, string? operatorId, string? UserId);
    }
}
