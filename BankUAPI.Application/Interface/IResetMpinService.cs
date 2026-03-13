using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IResetMpinService
    {
        Task<ResetMpinResponse> ResetMpinAsync(ResetMpin model, CancellationToken cn);
    }
}
