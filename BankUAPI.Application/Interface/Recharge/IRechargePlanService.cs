using BankUAPI.SharedKernel.Request.Recharge;
using BankUAPI.SharedKernel.Response.Recharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Recharge
{
    public interface IRechargePlanService
    {
        Task<RechargePlanResponse> GetRechargePlans(RechargePlanRequest request, CancellationToken ct);
        Task<CircleResponse> GetCircles(CircleRequest request, CancellationToken ct);
    }
}
