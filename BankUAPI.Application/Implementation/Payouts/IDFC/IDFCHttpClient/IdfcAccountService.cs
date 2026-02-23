using BankUAPI.Application.IDFCPayout.Crypto;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using BankUAPI.SharedKernel.Helper.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC.IDFCHttpClient
{
    public sealed class IdfcAccountService : IIdfcAccountService
    {
        private readonly HttpClient _client;
        private readonly IIdfcAuthService _auth;
        private readonly IdfcBankOptions _options;
        private readonly IIdempotencyService _idempotency;
        private readonly ApiLogger _logger;
        private readonly TransactionLedgerService _ledger;

        public IdfcAccountService(
        IHttpClientFactory factory,
        IIdfcAuthService auth,
        IOptions<IdfcBankOptions> options,
        IIdempotencyService idempotency,
        ApiLogger logger,
        TransactionLedgerService ledger)
        {
            _client = factory.CreateClient("IDFCClient");
            _auth = auth;
            _options = options.Value;
            _idempotency = idempotency;
            _logger = logger;
            _ledger = ledger;
        }

        public async Task<IdfcAccountBalanceResponse> GetAccountBalanceAsync(string idempotencyKey, string clientCode)
        {
            var internalTxnId = Guid.NewGuid().ToString("N");

            var payload = JsonSerializer.Serialize(new
            {
                prefetchAccountReq = new
                {
                    CBSTellerBranch = "",
                    CBSTellerID = "",
                    accountNumber = _options.DebitAccount.Trim()
                }
            });


            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload)));


            var cached = await _idempotency
                .GetExistingResponseAsync(idempotencyKey, hash, clientCode);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<IdfcAccountBalanceResponse>(cached)!;
            }

            var token = await _auth.GetAccessTokenAsync();
            var encrypted = AesCryptoService.Encrypt(payload, _options.SecretKey);

            var req = new HttpRequestMessage(
                HttpMethod.Post, _options.AccountBalance);

            req.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            req.Headers.Add("source", _options.Source);
            req.Headers.Add("correlationId", internalTxnId);

            var content = new ByteArrayContent(
              Encoding.UTF8.GetBytes(encrypted));

            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            req.Content = content;


            var sw = Stopwatch.StartNew();
            var res = await _client.SendAsync(req);
            var encRes = await res.Content.ReadAsStringAsync();
            sw.Stop();
            await _logger.LogAsync(
               req, res, payload+ encrypted, encRes,
               "IDFC-GetBalance", clientCode, sw.ElapsedMilliseconds);

            var decrypted = AesCryptoService.Decrypt(encRes, _options.SecretKey);

            await _logger.LogAsync(
                req, res, payload, decrypted,
                "IDFC-GetBalance", clientCode, sw.ElapsedMilliseconds);

            await _ledger.RecordBalanceAsync(
                clientCode, internalTxnId, _options.DebitAccount.Trim(),
                res.IsSuccessStatusCode ? "SUCCESS" : "FAILED",
                payload, decrypted, "BalanceCheck", "", "BalanceCheck");

            res.EnsureSuccessStatusCode();

            await _idempotency.SaveAsync(
                idempotencyKey, hash, decrypted, clientCode);

            return JsonSerializer.Deserialize<IdfcAccountBalanceResponse>(
                decrypted,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                })!;
        }


    }
}
