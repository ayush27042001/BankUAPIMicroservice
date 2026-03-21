using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IAgreementSignService
    {
        Task<ApiResponse<AadhaarOtpResponse>> SendAadhaarOtpAsync(AadhaarRequest request, CancellationToken cn);

        Task<ApiResponse<AgreementSignResult>> VerifyAndSignAsync(VerifyOtpRequest request, CancellationToken cn);
    }
}
