using BankUAPI.Application.IDFCPayout.Crypto;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using BankUAPI.SharedKernel.Helper.Payout.IDFC;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using Microsoft.Extensions.Options;
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
    public sealed class IdfcAccountStatementService : IIdfcAccountStatementService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IIdfcAuthService _auth;
        private readonly IdfcBankOptions _options;
        private readonly IIdempotencyService _idempotency;
        private readonly ApiLogger _logger;
        private readonly TransactionLedgerService _ledger;

        public IdfcAccountStatementService(
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

        public async Task<AccountStatementResponse> GetMiniStatementAsync(
    AccountStatementRequest request,
    string idempotencyKey,
    string clientCode)
        {

            var requestPayload = new
            {
                getAccountStatementReq = new
                {
                    CBSTellerBranch = request.getAccountStatementReq.CBSTellerBranch,
                    CBSTellerID = request.getAccountStatementReq.CBSTellerID,
                    accountNumber = _options.DebitAccount.Trim(),
                    fromDate = request.getAccountStatementReq.fromDate,
                    toDate = request.getAccountStatementReq.toDate,
                    numberOfTransactions = request.getAccountStatementReq.numberOfTransactions,
                    prompt = request.getAccountStatementReq.prompt
                }
            };

            var requestJson = JsonSerializer.Serialize(requestPayload, IdfcJson.CamelCase);
            var requestHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(requestJson)));
           
            var cached = await _idempotency
                .GetExistingResponseAsync(idempotencyKey, requestHash, clientCode);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<AccountStatementResponse>(
                    cached, IdfcJson.CamelCase)!;
            }

            var token = await _auth.GetAccessTokenAsync();
            var client = _httpFactory.CreateClient("IDFCClient");

            var encryptedPayload =
                AesCryptoService.Encrypt(requestJson, _options.SecretKey);

            var correlationId = Guid.NewGuid().ToString("N");

            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post, _options.AccountStatement);

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Headers.Add("source", _options.Source);
            httpRequest.Headers.Add("correlationId", correlationId);

            var content = new ByteArrayContent(
                Encoding.UTF8.GetBytes(encryptedPayload));

            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            httpRequest.Content = content;

            var sw = Stopwatch.StartNew();
            var response = await client.SendAsync(httpRequest);
            var encryptedResponse = await response.Content.ReadAsStringAsync();
            sw.Stop();

            await _logger.LogAsync(
                httpRequest,
                response,
                encryptedPayload,
                encryptedResponse,
                "IDFC_MINI_STATEMENT",
                clientCode,
                sw.ElapsedMilliseconds);

            response.EnsureSuccessStatusCode();

            var decryptedJson =
                AesCryptoService.Decrypt(encryptedResponse, _options.SecretKey);

            await _idempotency.SaveAsync(
                idempotencyKey,
                requestHash,
                decryptedJson,
                clientCode);

            var result = JsonSerializer.Deserialize<AccountStatementResponse>(
                decryptedJson, IdfcJson.CamelCase)!;

            await _ledger.RecordBalanceAsync(
                clientCode: clientCode,
                internalTxnId: correlationId,
                debitAccount: _options.DebitAccount,
                status: result.getAccountStatementResp.metaData.status,
                requestJson: requestJson,
                responseJson: decryptedJson,
                TxnType: "MINI_STATEMENT",
                BankCode: "IDFC");

            return result;
        }
    }


}
