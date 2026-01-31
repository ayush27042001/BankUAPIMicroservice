using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.DMT.Provider
{
    public interface IDmtCommissionService
    {
        Task<CommissionResponse> ApplyAsync(decimal txnAmount, string serviceId, string providerId, string? operatorId, string? UserId);
    }
}
