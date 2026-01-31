using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.DMT.Provider;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Helper.DMT;
using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankUAPI.Application.Implementation.DMT.InstantPay
{
    public class InstantPayProvider : IDmtProvider
    {
        private readonly IInstantPayClient _client;
        private readonly IUserRepository _UserRepo;
        private readonly ICommonRepositry _repo;
        private readonly IDmtCommissionService _DMTCommision;

        public InstantPayProvider(IInstantPayClient client, IUserRepository userRepo, ICommonRepositry cr, IDmtCommissionService dMTCommision)
        {
            _client = client;
            _UserRepo = userRepo;
            _repo = cr;
            _DMTCommision = dMTCommision;
        }

        public async Task<RemitterProfileResponse> GetRemitterProfileAsync(RemitterProfileRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterProfileResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.GetRemitterProfileAsync(
                request,
                outletId: outletId,
                endpointIp: request.EndpointIp,
                ct
            );
        }


        public async Task<RemitterRegistrationResponse> GetRemitterRegistrationAsync(RemitterRegistrationRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterRegistrationResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.GetRemitterRegistrationAsync(
                request,
                outletId: outletId,
                endpointIp: request.EndpointIp,
                ct
            );
        }

        public async Task<RemitterRegistrationResponse> RemitterRegistrationValidateAsync(RemitterRegistrationRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterRegistrationResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.RemitterRegistrationValidateAsync(
                request,
                outletId: outletId,
                endpointIp: request.EndpointIp,
                ct
            );
        }
        public async Task<RemitterEKYCResponse> RegisterEKYCAsync(RemitterEKYC request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterEKYCResponse
                {
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            request.ExternalRef = ExternalRefGenerator.Generate();

            return await _client.RegisterEKYCAsync(
                request,
                outletId,
                ct
            );
        }

        public async Task<RemitterBenificiaryResponse> BeneficiaryRegistraton(RemitterBeneficiaryRegistration request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterBenificiaryResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.RegisterBeneficiaryAsync(
                request,
                outletId,
                ct
            );
        }

        public async Task<RemitterBenificiaryResponse> BeneficiaryRegistratonVerification(BeneficiaryVerifyRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterBenificiaryResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.BeneficiaryVerify(
                request,
                outletId,
                ct
            );
        }

        public async Task<RemitterBenificiaryResponse> DeleteBeneficiary(BeneficiaryDeleteRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterBenificiaryResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.BeneficiaryDelete(
                request,
                outletId,
                ct
            );
        }

        public async Task<RemitterBenificiaryResponse> DeleteBeneficiaryVerify(BeneficiaryVerifyRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterBenificiaryResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }
            return await _client.BeneficiaryDeleteVerify(
                request,
                outletId,
                ct
            );
        }

        public async Task<InsPayBankListResponse> FetchBankListAndSyncInDB(string endpointIp, string userId, CancellationToken ct)
        {
            if (!int.TryParse(userId, out int parsedUserId))
            {
                return new InsPayBankListResponse
                {
                    StatusCode = "400",
                    Status = "Invalid UserId"
                };
            }

            var outletId = await _UserRepo.GetOutletIdAsync(parsedUserId, ct);

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new InsPayBankListResponse
                {
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            var apiResponse = await _client.FetchBankList(
                outletId,
                endpointIp,
                ct
            );

            if (apiResponse == null || apiResponse.Success != true || apiResponse.Data == null)
            {
                return new InsPayBankListResponse
                {
                    StatusCode = apiResponse?.StatusCode ?? "ERR",
                    Status = apiResponse?.Status ?? "Failed to fetch bank list"
                };
            }

            foreach (var item in apiResponse.Data)
            {
                var bankEntity = new INSPayBankDetail
                {
                    bankId = item.BankId,
                    name = item.Name,
                    ifscAlias = item.IfscAlias,
                    ifscGlobal = item.IfscGlobal,

                    neftEnabled = item.NeftEnabled,
                    neftFailureRate = item.NeftFailureRate,

                    impsEnabled = item.ImpsEnabled,
                    impsFailureRate = item.ImpsFailureRate,

                    upiEnabled = item.UpiEnabled,
                    upiFailureRate = item.UpiFailureRate
                };

                await _repo.InsertInsPayBankDetails(bankEntity, ct);
            }

            return new InsPayBankListResponse
            {
                StatusCode = "TXN",
                Status = "Bank details synced successfully"
            };
        }

        public async Task<List<INSPayBankDetail>> FetchBankList(CancellationToken ct)
        {
            var Data = await _repo.FetchBankDetails();
            return Data;
        }

        public async Task<DmtTransactionResponse> DMTTransaction(DmtTransactionRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );
            request.externalRef = ExternalRefGenerator.Generate();
            Registration? UserData = await _UserRepo.GetUserData(request?.UserId, ct);

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new DmtTransactionResponse
                {
                    statuscode = "401",
                    status = "OutletId not found, Please complete your onboarding!"
                };
            }

            if (!string.IsNullOrWhiteSpace(request.transferAmount))
            {
                if (!decimal.TryParse(request.transferAmount, out decimal amount))
                {
                    return new DmtTransactionResponse
                    {
                        statuscode = "ERR",
                        status = "Invalid transfer amount"
                    };
                }

                if (amount < 100 || amount > 5000)
                {
                    return new DmtTransactionResponse
                    {
                        statuscode = "ERR",
                        status = "Please enter amount between 100 and 5000"
                    };
                }
            }

            var walletcheck = await _repo.WalletCheckValidationAsync( Convert.ToInt32(request?.UserId), Convert.ToDecimal(request.transferAmount), ct);
            if(!walletcheck.success)
            {
                return new DmtTransactionResponse
                {
                    statuscode = "402",
                    status = walletcheck.message
                };
            }
            var Data= await _client.DoDmtTransactionAsync(
                request,
                outletId,
                ct
            );

            if (Data.statuscode == "TXN" || Data.statuscode== "TUP")
            {
                var data=await _DMTCommision.ApplyAsync(
                    decimal.Parse(Data.data.txnValue),
                    "DMT",
                    "INP",
                    null,
                    request.UserId
                );

                await _repo.AddDmtTxnReportAsync(UserData, "DMT", Data?.orderid, "DMT_Transaction", request.remitterMobileNumber, request.accountNumber, Convert.ToDecimal(request.transferAmount), data?.RetailerCommissionCharge??0, Data?.statuscode == "TXN" ? "SUCCESS" : Data?.statuscode=="TUP"?"PROCESS":"FAILED", "InsPay", Data, request.externalRef, Data?.data?.poolReferenceId, "", Data?.data?.beneficiaryName, Data?.data?.beneficiaryIfsc, data?.WalletAmount, ct);
                return Data;
            }

            await _repo.AddDmtTxnReportAsync(UserData, "DMT", Data?.orderid, "DMT_Transaction", request.remitterMobileNumber, request.accountNumber, Convert.ToDecimal(request.transferAmount), 0,  "FAILED", "InsPay", Data, request.externalRef, Data?.data?.poolReferenceId, "", Data?.data?.beneficiaryName, Data?.data?.beneficiaryIfsc, 0, ct);
            return Data;
        }

        public async Task<GenerateTranSactionOTPResponse> DMTTransactionOTPGenerate(GenerateTransactionOTPRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.userId),
                ct
            );
           
            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new GenerateTranSactionOTPResponse
                {
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            if (!string.IsNullOrWhiteSpace(request.amount))
            {
                if (!decimal.TryParse(request.amount, out decimal amount))
                {
                    return new GenerateTranSactionOTPResponse
                    {
                        StatusCode = "ERR",
                        Status = "Invalid transfer amount"
                    };
                }

                if (amount < 100 || amount > 5000)
                {
                    return new GenerateTranSactionOTPResponse
                    {
                        StatusCode = "ERR",
                        Status = "Please enter amount between 100 and 5000"
                    };
                }
            }

            var walletcheck = await _repo.WalletCheckValidationAsync(Convert.ToInt32(request?.userId), Convert.ToDecimal(request.amount), ct);
            if (!walletcheck.success)
            {
                return new GenerateTranSactionOTPResponse
                {
                    StatusCode = "402",
                    Status = walletcheck.message
                };
            }

            var Data = await _client.DMTTransactionSendOTP(
                request,
                outletId,
                ct
            );

            return Data;
        }

    }
}
