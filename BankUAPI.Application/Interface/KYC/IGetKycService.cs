using BankUAPI.SharedKernel.Response.KYC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.KYC
{
    public interface IGetKycService
    {
        Task<GetKycResponse> GetKycAsync(string registrationId, CancellationToken cn);
    }
}
