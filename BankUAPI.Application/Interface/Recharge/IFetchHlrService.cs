using BankUAPI.SharedKernel.Request.Recharge;
using BankUAPI.SharedKernel.Response.Recharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Recharge
{
    public interface IFetchHlrService
    {
        Task<FetchHlrResponse> FetchDetails(FetchHlrRequest request, CancellationToken ct);
    }
}
