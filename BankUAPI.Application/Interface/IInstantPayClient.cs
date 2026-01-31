using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IInstantPayClient
    {
        Task<string> SignupAsync(object payload, CancellationToken ct);
        Task<string> CheckStatusAsync(string outletId, CancellationToken ct);
        Task<string> SignupValidateAsync(object payload, CancellationToken ct);
        ValueTask<BiometricKycStatusApiResponse> GetBiometricKycStatusAsync(string outletId, CancellationToken ct);
        ValueTask<BiometricKycApiResponse> SubmitBiometricKycAsync(string outletId, BiometricKycApiPayload payload, CancellationToken ct);
        ValueTask<AepsLoginApiResponse> AepsDailyLoginAsync(string outletId, AepsLoginApiPayload payload,CancellationToken ct);
        ValueTask<AepsBapApiResponse> BalanceEnquiryAsync(string outletId, AepsBapApiRequest request,CancellationToken ct);
        ValueTask<AepsSapApiResponse> MiniStatementAsync(string outletId, AepsSapApiRequest request, CancellationToken ct);


        //DMT Calls Start
        Task<RemitterProfileResponse> GetRemitterProfileAsync(RemitterProfileRequest request, string outletId, string endpointIp, CancellationToken ct);
        Task<RemitterRegistrationResponse> GetRemitterRegistrationAsync(RemitterRegistrationRequest request, string outletId, string endpointIp, CancellationToken ct);
        Task<RemitterRegistrationResponse> RemitterRegistrationValidateAsync(RemitterRegistrationRequest request, string outletId, string endpointIp, CancellationToken ct);
        Task<RemitterEKYCResponse> RegisterEKYCAsync(RemitterEKYC request, string outletId, CancellationToken ct);
        Task<RemitterBenificiaryResponse> RegisterBeneficiaryAsync(RemitterBeneficiaryRegistration request, string outletId, CancellationToken ct);
        Task<RemitterBenificiaryResponse> BeneficiaryVerify(BeneficiaryVerifyRequest request, string outletId, CancellationToken ct);
        Task<RemitterBenificiaryResponse> BeneficiaryDelete(BeneficiaryDeleteRequest request, string outletId, CancellationToken ct);
        Task<RemitterBenificiaryResponse> BeneficiaryDeleteVerify(BeneficiaryVerifyRequest request, string outletId, CancellationToken ct);
        Task<InsPayBankListResponse> FetchBankList(string outletId, string IP, CancellationToken ct);
        Task<DmtTransactionResponse> DoDmtTransactionAsync(DmtTransactionRequest request, string outletId, CancellationToken ct);
        Task<GenerateTranSactionOTPResponse> DMTTransactionSendOTP(GenerateTransactionOTPRequest request, string outletId, CancellationToken ct);
    }

}
 