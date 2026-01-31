using BankUAPI.Application.IDFCPayout.Crypto;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using BankUAPI.SharedKernel.Helper.Payout.IDFC;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC.IDFCHttpClient
{
    public sealed class IdfcBeneValidationService : IIdfcBeneValidationService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IIdfcAuthService _auth;
        private readonly IdfcBankOptions _options;
        private readonly IIdempotencyService _idempotency;
        private readonly ApiLogger _logger;
        private readonly TransactionLedgerService _ledger;

        public IdfcBeneValidationService(
            IHttpClientFactory httpFactory,
            IIdfcAuthService auth,
            IOptions<IdfcBankOptions> options,
            IIdempotencyService idempotency,
            ApiLogger logger,
            TransactionLedgerService ledger)
        {
            _httpFactory = httpFactory;
            _auth = auth;
            _options = options.Value;
            _idempotency = idempotency;
            _logger = logger;
            _ledger = ledger;
        }

        public async Task<BeneValidationResponse> ValidateAsync(
            BeneValidationRequest request,
            string idempotencyKey,
            string clientCode)
        {
            var requestJson = JsonSerializer.Serialize(request, IdfcJson.CamelCase);
            var requestHash = SHA256.HashData(Encoding.UTF8.GetBytes(requestJson))
                .ToString();

            var cached = await _idempotency
                .GetExistingResponseAsync(idempotencyKey, requestHash, clientCode);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<BeneValidationResponse>(cached)!;
            }

            var token = await _auth.GetAccessTokenAsync();
            var client = _httpFactory.CreateClient("IDFCClient");

            var encryptedPayload =
                AesCryptoService.Encrypt(requestJson, _options.SecretKey);

            var correlationId = Guid.NewGuid().ToString("N");

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post, _options.BeneValidation);

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Headers.Add("source", _options.Source);
            httpRequest.Headers.Add("correlationId", correlationId);

            httpRequest.Content = new StringContent(
                encryptedPayload,
                Encoding.UTF8,
                "application/octet-stream");

            await _logger.LogAsync(
                httpRequest, null, requestJson, "",
                "IDFC-BeneValidation", clientCode, 0);

            var sw = Stopwatch.StartNew();
            var response = await client.SendAsync(httpRequest);
            var encryptedResponse = await response.Content.ReadAsStringAsync();
            sw.Stop();
            var decryptedJson =
                AesCryptoService.Decrypt(encryptedResponse, _options.SecretKey);

            await _logger.LogAsync(
                httpRequest, response, requestJson, decryptedJson,
                "IDFC-BeneValidation", clientCode, sw.ElapsedMilliseconds);

            await _idempotency.SaveAsync(
               idempotencyKey, requestHash, decryptedJson, clientCode);


            var result = JsonSerializer.Deserialize<BeneValidationResponse>(
                decryptedJson)!;

            await _ledger.RecordBalanceAsync(
               clientCode, correlationId, request?.beneValidationReq?.debtorAccountId,
               result?.beneValidationResp?.metaData?.status,
               requestJson, decryptedJson, "BENE_VALIDATION", "", "BENE_VALIDATION", request?.beneValidationReq?.transactionReferenceNumber, request?.beneValidationReq?.creditorAccountId);

           
            return result;
        }
    }

}
