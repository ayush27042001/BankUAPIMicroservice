using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.DMT.Provider
{
    public interface IDmtProvider
    {
        Task<RemitterProfileResponse> GetRemitterProfileAsync(RemitterProfileRequest request, CancellationToken ct);
    }
}
