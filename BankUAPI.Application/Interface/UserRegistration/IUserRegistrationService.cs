using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.UserRegistration
{
    public interface IUserRegistrationService
    {
        Task<ApiResponse<Step1Response>> Step1(Step1Request request, CancellationToken ct);
        Task<ApiResponse<MessageResponse>> Step2(Step2Request request, CancellationToken ct);
        Task<ApiResponse<MessageResponse>> Step3(Step3Request request, CancellationToken ct);
        Task<ApiResponse<MessageResponse>> Step4(Step4Request request, CancellationToken ct);
        Task<ApiResponse<MessageResponse>> Complete(Step5Request request, CancellationToken ct);
        Task<ApiResponse<RegistrationStatusResponse>> GetRegistrationStatus(RegistrationStatusRequest req, CancellationToken ct);
        Task<ApiResponse<AadhaarOtpResult>> SendAadhaarOtp(AadhaarOtpRequest req, CancellationToken ct);
        Task<ApiResponse<MpinLookupResponse>> GetMpinByMobile(MpinLookupRequest req, CancellationToken ct);
    }
}
