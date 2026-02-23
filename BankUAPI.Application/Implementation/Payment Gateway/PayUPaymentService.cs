using BankUAPI.Application.Interface.Payment_Gateway.PayU;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.PayU;
using BankUAPI.SharedKernel.Helper.Payment_Gateway.PayU;
using BankUAPI.SharedKernel.Request.Payment_Gateway.PayU;
using BankUAPI.SharedKernel.Response.Payment_Gateway.PayU;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payment_Gateway
{
    public class PayUPaymentService : IPayUPaymentService
    {
        private readonly HttpClient _http;
        private readonly PayUSettings _settings;
        private readonly AppDbContext _dbContext;

        public PayUPaymentService(IHttpClientFactory factory, IOptions<PayUSettings> settings, AppDbContext dbcontext)
        {
            _http = factory.CreateClient("PayUClient");
            _settings = settings.Value;
            _dbContext = dbcontext;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest req)
        {

            var existing = await _dbContext.Payments.FirstOrDefaultAsync(x => x.TxnId == req.TxnId);
            if (existing != null)
            {
                return new PaymentResponse
                {
                    TxnId = existing.TxnId,
                    PayUMoneyId = existing.PayUMihPayId,
                    Status = existing.Status,
                    BankRef = existing.BankRefNumber,
                    Message = "Duplicate Request - Existing Transaction Returned"
                };
            }

            // Insert Pending
            var payment = new Payment
            {
                TxnId = req.TxnId,
                Amount = req.Amount,
                Status = "PENDING",
                CustomerEmail = req.Email,
                CustomerPhone = req.Phone,
                Last4Card = req.CardNumber[^4..],
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();

            var hash = PayUHashGenerator.GenerateHash(
                _settings.SaltKey,
                req.TxnId,
                req.Amount.ToString("0.00"),
                req.ProductInfo,
                req.FirstName,
                req.Email,
                _settings.Salt
            );

            var postData = new Dictionary<string, string>
            {
                ["key"] = _settings.SaltKey,
                ["txnid"] = req.TxnId,
                ["amount"] = req.Amount.ToString("0.00"),
                ["productinfo"] = req.ProductInfo,
                ["firstname"] = req.FirstName,
                ["email"] = req.Email,
                ["phone"] = req.Phone,
                ["pg"] = "CC",
                ["bankcode"] = "CC",
                ["ccnum"] = req.CardNumber,
                ["ccname"] = req.CardName,
                ["ccvv"] = req.CVV,
                ["ccexpmon"] = req.ExpiryMonth,
                ["ccexpyr"] = req.ExpiryYear,
                ["txn_s2s_flow"] = _settings.S2SFlow,
                ["surl"] = "https://bankupartner.co.in/success",
                ["furl"] = "https://bankupartner.co.in/fail",
                ["hash"] = hash
            };

            var response = await _http.PostAsync(
                $"{_settings.BaseUrl}/_payment",
                new FormUrlEncodedContent(postData)
            );

            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);

            return new PaymentResponse
            {
                TxnId = result?.result!=null?result?.result?.txnid: "",
                PayUMoneyId = result?.result != null ? result?.result?.mihpayid : "",
                Status = result?.result != null ? result?.result?.status : "",
                BankRef = result?.result != null ? result?.result?.bank_ref_num : "",
                Message = result?.message
            };
        }

        public async Task<bool> VerifyPaymentAsync(string txnId)
        {
            string command = "verify_payment";
            string hashString = $"{_settings.SaltKey}|{command}|{txnId}|{_settings.Salt}";

            using var sha512 = SHA512.Create();
            var hash = BitConverter.ToString(
                sha512.ComputeHash(Encoding.UTF8.GetBytes(hashString))
            ).Replace("-", "").ToLower();

            var form = new Dictionary<string, string>
            {
                ["key"] = _settings.SaltKey,
                ["command"] = command,
                ["var1"] = txnId,
                ["hash"] = hash
            };

            var response = await _http.PostAsync(_settings.VerifyUrl, new FormUrlEncodedContent(form));
            var content = await response.Content.ReadAsStringAsync();

            return content.Contains("captured") || content.Contains("success");
        }

    }
}
