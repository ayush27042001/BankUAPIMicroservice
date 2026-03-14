using BankUAPI.SharedKernel.Request.KYC;
using BankUAPI.SharedKernel.Response.KYC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.KYC
{
    public interface IAddKycService
    {
        Task<KycResponse> UploadKycAsync(AddKycRequest request, CancellationToken cn);
    }
}
