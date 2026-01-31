using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BankUAPI.Application.Implementation
{
    public sealed class InstantPayClient : IInstantPayClient
    {
        private static readonly JsonSerializerOptions _jsonOpt = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _http;
        private readonly InstantPayOptions _opt;
        private readonly ICommonRepositry _commonRepo;

        public InstantPayClient(
            IHttpClientFactory factory,
            IOptions<InstantPayOptions> options, ICommonRepositry commonRepo)
        {
            _http = factory.CreateClient("InsPay");
            _opt = options.Value;
            _commonRepo = commonRepo;
        }

        public async Task<string> SignupAsync(object payload, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/aeps/outlet/signup");

            request.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            request.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            request.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);

            request.Content = JsonContent.Create(payload);

            using var response = await _http.SendAsync(request, ct);
            return await response.Content.ReadAsStringAsync(ct);
        }

        public async Task<string> CheckStatusAsync(string outletId, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/aeps/outletLoginStatus");

            request.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            request.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            request.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            request.Headers.Add("X-Ipay-Outlet-Id", outletId);

            request.Content = JsonContent.Create(new
            {
                type = "CashDeposit"
            });

            using var response = await _http.SendAsync(request, ct);
            return await response.Content.ReadAsStringAsync(ct);
        }

        public async Task<string> SignupValidateAsync(object payload, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/aeps/outlet/signup/validate");

            request.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            request.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            request.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);

            request.Content = JsonContent.Create(payload);

            using var response = await _http.SendAsync(request, ct);
            return await response.Content.ReadAsStringAsync(ct);
        }

        public async ValueTask<BiometricKycStatusApiResponse>GetBiometricKycStatusAsync(string outletId, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{_opt.BaseUrl}/user/outlet/signup/biometricKycStatus");
            request.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            request.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            request.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            request.Headers.Add("X-Ipay-Outlet-Id", outletId);

            request.Content = JsonContent.Create(
                new { spkey = "DMI" },
                options: _jsonOpt);

            using var resp = await _http.SendAsync(request,ct);
            resp.EnsureSuccessStatusCode();
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<
                BiometricKycStatusApiResponse>(stream, _jsonOpt, ct);
        }

        public async ValueTask<BiometricKycApiResponse>SubmitBiometricKycAsync(string outletId, BiometricKycApiPayload payload, CancellationToken ct)
        {
            using var req = new HttpRequestMessage( HttpMethod.Post, $"{_opt.BaseUrl}/user/outlet/signup/biometricKyc");
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Content = JsonContent.Create(payload, options: _jsonOpt);
            using var resp = await _http.SendAsync(req, ct);
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<
                BiometricKycApiResponse>(stream, _jsonOpt, ct);
        }

        public async ValueTask<AepsLoginApiResponse>AepsDailyLoginAsync(string outletId, AepsLoginApiPayload payload,CancellationToken ct)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{_opt.BaseUrl}/fi/aeps/outletLogin");
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Content = JsonContent.Create(payload, options: _jsonOpt);
            using var resp = await _http.SendAsync(req, ct);
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<
                AepsLoginApiResponse>(stream, _jsonOpt, ct);
        }

        public async ValueTask<AepsBapApiResponse>BalanceEnquiryAsync(string outletId, AepsBapApiRequest request, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, $"{_opt.BaseUrl}/fi/aeps/balanceInquiry");
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Content = JsonContent.Create(request, options: _jsonOpt);
            using var resp = await _http.SendAsync(req, ct);
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<
                AepsBapApiResponse>(stream, _jsonOpt, ct);
        }

        public async ValueTask<AepsSapApiResponse> MiniStatementAsync(string outletId, AepsSapApiRequest request, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/aeps/miniStatement");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Content = JsonContent.Create(request, options: _jsonOpt);
            using var resp = await _http.SendAsync(req, ct);
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync<AepsSapApiResponse>(
                stream, _jsonOpt, ct);
        }


        //DMT Calls Starts
        public async Task<RemitterProfileResponse> GetRemitterProfileAsync(RemitterProfileRequest request, string outletId, string endpointIp, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/remitterProfile");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", endpointIp);

            req.Content = JsonContent.Create(new
            {
                mobileNumber = request.MobileNumber,
                txnMode = request.TxnMode,
                iftEnable = request.IftEnable
            }, options: _jsonOpt);

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

            resp.EnsureSuccessStatusCode();

            var resData = await resp.Content.ReadAsStringAsync(ct);

          
            await using var stream = await resp.Content.ReadAsStreamAsync(ct);

            var remitterResponse = await JsonSerializer.DeserializeAsync<RemitterProfileResponse>(
                stream,
                _jsonOpt,
                ct
            );

           

            return remitterResponse!;
        }

        public async Task<RemitterRegistrationResponse> GetRemitterRegistrationAsync(RemitterRegistrationRequest request, string outletId, string endpointIp, CancellationToken ct)
        {
            
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/remitterRegistration");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", endpointIp);

            req.Content = JsonContent.Create(new
            {
                mobileNumber = request.mobileNumber,
                encryptedAadhaar = _commonRepo.AESEncryption(request.Aadhar),
                referenceKey = request.referenceKey
            }, options: _jsonOpt);

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

            resp.EnsureSuccessStatusCode();

            var resData = await resp.Content.ReadAsStringAsync(ct);

            await using var stream = await resp.Content.ReadAsStreamAsync(ct);

            var remitterResponse = await JsonSerializer.DeserializeAsync<RemitterRegistrationResponse>(
                stream,
                _jsonOpt,
                ct
            );



            return remitterResponse!;
        }

        public async Task<RemitterRegistrationResponse> RemitterRegistrationValidateAsync(RemitterRegistrationRequest request, string outletId, string endpointIp, CancellationToken ct)
        {
           //
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/remitterRegistrationVerify");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", endpointIp);

            req.Content = JsonContent.Create(new
            {
                mobileNumber = request.mobileNumber,
                otp = request.otp,
                referenceKey = request.referenceKey
            }, options: _jsonOpt);

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

            

            var resData = await resp.Content.ReadAsStringAsync(ct);

            await using var stream = await resp.Content.ReadAsStreamAsync(ct);

            var remitterResponse = await JsonSerializer.DeserializeAsync<RemitterRegistrationResponse>(
                stream,
                _jsonOpt,
                ct
            );

            return remitterResponse!;
        }

        public async Task<RemitterEKYCResponse> RegisterEKYCAsync(RemitterEKYC request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/remitterKyc");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.EndpointIp);

            req.Content = JsonContent.Create(new
            {
                mobileNumber = request.MobileNumber,
                referenceKey = request.ReferenceKey,
                latitude = request.Latitude,
                longitude = request.Longitude,
                externalRef = request.ExternalRef,
                consentTaken = request.ConsentTaken,
                captureType = request.CaptureType,
                biometricData = request.BiometricData
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<RemitterEKYCResponse>(json, _jsonOpt);

            return result ?? new RemitterEKYCResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<RemitterBenificiaryResponse> RegisterBeneficiaryAsync(RemitterBeneficiaryRegistration request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/beneficiaryRegistration");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.EndpointIp);

            req.Content = JsonContent.Create(new
            {
                beneficiaryMobileNumber = request.beneficiaryMobileNumber,
                remitterMobileNumber = request.remitterMobileNumber,
                accountNumber = request.accountNumber,
                ifsc = request.ifsc,
                bankId = request.bankId,
                name = request.name
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<RemitterBenificiaryResponse>(json, _jsonOpt);

            return result ?? new RemitterBenificiaryResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<RemitterBenificiaryResponse> BeneficiaryVerify(BeneficiaryVerifyRequest request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/beneficiaryRegistrationVerify");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.EndpointIp);

            req.Content = JsonContent.Create(new
            {
                remitterMobileNumber = request.remitterMobileNumber,
                referenceKey = request.referenceKey,
                otp = request.otp,
                beneficiaryId = request.beneficiaryId,
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<RemitterBenificiaryResponse>(json, _jsonOpt);

            return result ?? new RemitterBenificiaryResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<RemitterBenificiaryResponse> BeneficiaryDelete(BeneficiaryDeleteRequest request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/beneficiaryDelete");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.EndpointIp);

            req.Content = JsonContent.Create(new
            {
                remitterMobileNumber = request.remitterMobileNumber,
                beneficiaryId = request.beneficiaryId
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<RemitterBenificiaryResponse>(json, _jsonOpt);

            return result ?? new RemitterBenificiaryResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<RemitterBenificiaryResponse> BeneficiaryDeleteVerify(BeneficiaryVerifyRequest request, string outletId, CancellationToken ct)
        {

            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/beneficiaryDeleteVerify");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.EndpointIp);

            req.Content = JsonContent.Create(new
            {
                remitterMobileNumber = request.remitterMobileNumber,
                referenceKey = request.referenceKey,
                otp= request.otp,
                beneficiaryId = request.beneficiaryId
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<RemitterBenificiaryResponse>(json, _jsonOpt);

            return result ?? new RemitterBenificiaryResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<InsPayBankListResponse> FetchBankList(string outletId, string IP, CancellationToken ct)
        {

            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/banks");

            // Headers
            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", IP);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<InsPayBankListResponse>(json, _jsonOpt);

            return result ?? new InsPayBankListResponse
            {
                StatusCode = "ERR",
                Status = "Invalid response from InstantPay"
            };
        }

        public async Task<DmtTransactionResponse> DoDmtTransactionAsync(DmtTransactionRequest request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/transaction");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.IP);

            req.Content = JsonContent.Create(new
            {
                request.remitterMobileNumber,
                request.accountNumber,
                request.ifsc,
                request.transferMode,
                request.transferAmount,
                request.latitude,
                request.longitude,
                request.referenceKey,
                request.otp,
                request.externalRef
            }, options: _jsonOpt);

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);

            return JsonSerializer.Deserialize<DmtTransactionResponse>(json, _jsonOpt)
                   ?? throw new Exception("Invalid DMT response");
        }

        public async Task<GenerateTranSactionOTPResponse> DMTTransactionSendOTP(GenerateTransactionOTPRequest request, string outletId, CancellationToken ct)
        {
            using var req = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_opt.BaseUrl}/fi/remit/out/domestic/v2/generateTransactionOtp");

            req.Headers.Add("X-Ipay-Auth-Code", _opt.AuthCode);
            req.Headers.Add("X-Ipay-Client-Id", _opt.ClientId);
            req.Headers.Add("X-Ipay-Client-Secret", _opt.ClientSecret);
            req.Headers.Add("X-Ipay-Outlet-Id", outletId);
            req.Headers.Add("X-Ipay-Endpoint-Ip", request.ip);

            var payload = new
            {
                request.remitterMobileNumber,
                request.amount,
                request.referenceKey
            };

            var requestJson = JsonSerializer.Serialize(payload, _jsonOpt);
            req.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using var response = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var json = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<GenerateTranSactionOTPResponse>(json, _jsonOpt)
                   ?? throw new Exception("Invalid DMT response");
        }
    }
}
