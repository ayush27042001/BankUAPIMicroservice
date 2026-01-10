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
        private readonly AppDbContext _dbContext;

        public InstantPayClient(
            IHttpClientFactory factory,
            IOptions<InstantPayOptions> options, AppDbContext db)
        {
            _http = factory.CreateClient("InsPay");
            _opt = options.Value;
            _dbContext = db;
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

            await using var stream = await resp.Content.ReadAsStreamAsync(ct);

            var remitterResponse = await JsonSerializer.DeserializeAsync<RemitterProfileResponse>(
                stream,
                _jsonOpt,
                ct
            );

            _dbContext.AepsrequestLogs.Add(new AepsrequestLog
            {
                ApiType = "DMT",
                Reqdate = DateTime.Now,
                Request = JsonSerializer.Serialize(request, _jsonOpt),
                Responce = JsonSerializer.Serialize(remitterResponse, _jsonOpt)
            });

            await _dbContext.SaveChangesAsync(ct);

            return remitterResponse!;
        }

    }
}
