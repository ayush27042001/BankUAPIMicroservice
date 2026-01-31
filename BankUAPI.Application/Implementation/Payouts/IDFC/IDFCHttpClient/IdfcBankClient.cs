using BankUAPI.Application.IDFCPayout.Crypto;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using BankUAPI.SharedKernel.Helper.Payout.IDFC;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using BankUAPI.SharedKernel.Response.Payout.IDFC;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC.IDFCHttpClient
{
    public sealed class IdfcBankClient : IIdfcBankClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IIdfcAuthService _auth;
        private readonly IIdempotencyService _idempotency;
        private readonly ApiLogger _logger;
        private readonly IdfcBankOptions _opt;
        private readonly TransactionLedgerService _ledger;

        private static readonly JsonSerializerSettings JsonSettings =
        new()
        {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
        };

        public IdfcBankClient(IHttpClientFactory factory, IIdfcAuthService auth, IIdempotencyService idempotency, ApiLogger logger, IOptions<IdfcBankOptions> opt, TransactionLedgerService ledger)
        {
            _factory = factory;
            _auth = auth;
            _idempotency = idempotency;
            _logger = logger;
            _opt = opt.Value;
            _ledger = ledger;
        }

        public async Task<FundTransferResponse> TransferAsync(FundTransferRequest request, Registration user, string idempotencyKey, CancellationToken ct)
        {
            var json = JsonConvert.SerializeObject(request, JsonSettings);
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(json)).ToString();

            var cached = await _idempotency
                .GetExistingResponseAsync(idempotencyKey, hash, request?.initiateAuthGenericFundTransferAPIReq?.companyCode);

            if (cached != null)
                return JsonConvert.DeserializeObject<FundTransferResponse>(cached)!;

            var token = await _auth.GetAccessTokenAsync();
            var encrypted = AesCryptoService.Encrypt(json, _opt.SecretKey);

            var client = _factory.CreateClient("IDFCClient");

            var req = new HttpRequestMessage(HttpMethod.Post, _opt.FundTransfer);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Headers.Add("source", _opt.Source);
            var correlationId = Guid.NewGuid().ToString("N");
            req.Headers.Add("correlationId", correlationId);

            req.Content = new StringContent(
                encrypted, Encoding.UTF8, "application/octet-stream");

            var sw = Stopwatch.StartNew();
            var res = await client.SendAsync(req, ct);
            var encRes = await res.Content.ReadAsStringAsync(ct);
            sw.Stop();

            await _logger.LogAsync(
                req, res, encrypted, encRes,
                "IDFC_FUND_TRANSFER", request.initiateAuthGenericFundTransferAPIReq?.companyCode, sw.ElapsedMilliseconds);

            var decrypted = AesCryptoService.Decrypt(encRes, _opt.SecretKey);

            await _idempotency.SaveAsync(
                idempotencyKey, hash, decrypted, request.initiateAuthGenericFundTransferAPIReq?.companyCode);

            var result = System.Text.Json.JsonSerializer.Deserialize<FundTransferResponse>(decrypted, IdfcJson.CamelCase )!;
            await _ledger.RecordBalanceAsync(
                clientCode: request.initiateAuthGenericFundTransferAPIReq.companyCode,
                internalTxnId: correlationId,
                debitAccount: request.initiateAuthGenericFundTransferAPIReq.debitAccountNumber,
                status: result?.initiateAuthGenericFundTransferAPIResp?.metaData?.status??"",
                requestJson: json,
                responseJson: decrypted,
                TxnType: "FUND_TRANSFER",
                BankCode: "IDFC",
                TransactionType: "FUND_TRANSFER",
                ExternalTxnId: result?.initiateAuthGenericFundTransferAPIResp?.resourceData?.transactionID??"",
                CreditAccount: request?.initiateAuthGenericFundTransferAPIReq?.creditAccountNumber??"",
                Amount: request?.initiateAuthGenericFundTransferAPIReq?.amount??"",
                BankResponseCode: result?.initiateAuthGenericFundTransferAPIResp?.resourceData?.transactionReferenceNo??"",
                BankResponseMessage: result?.initiateAuthGenericFundTransferAPIResp?.metaData?.message??""
                );

            return result!;
        }
    }

}
