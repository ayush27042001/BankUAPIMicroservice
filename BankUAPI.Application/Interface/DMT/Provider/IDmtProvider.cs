using BankUAPI.Infrastructure.Sql.Entities;
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
        Task<RemitterRegistrationResponse> GetRemitterRegistrationAsync(RemitterRegistrationRequest request, CancellationToken ct);
        Task<RemitterRegistrationResponse> RemitterRegistrationValidateAsync(RemitterRegistrationRequest request, CancellationToken ct);
        Task<RemitterEKYCResponse> RegisterEKYCAsync(RemitterEKYC request, CancellationToken ct);
        Task<RemitterBenificiaryResponse> BeneficiaryRegistraton(RemitterBeneficiaryRegistration request, CancellationToken ct);
        Task<RemitterBenificiaryResponse> BeneficiaryRegistratonVerification(BeneficiaryVerifyRequest request, CancellationToken ct);
        Task<RemitterBenificiaryResponse> DeleteBeneficiary(BeneficiaryDeleteRequest request, CancellationToken ct);
        Task<RemitterBenificiaryResponse> DeleteBeneficiaryVerify(BeneficiaryVerifyRequest request, CancellationToken ct);
        Task<InsPayBankListResponse> FetchBankListAndSyncInDB(string endpointIp, string userId, CancellationToken ct);
        Task<List<INSPayBankDetail>> FetchBankList(CancellationToken ct);
        Task<DmtTransactionResponse> DMTTransaction(DmtTransactionRequest request, CancellationToken ct);
        Task<GenerateTranSactionOTPResponse> DMTTransactionOTPGenerate(GenerateTransactionOTPRequest request, CancellationToken ct);
    }
}
