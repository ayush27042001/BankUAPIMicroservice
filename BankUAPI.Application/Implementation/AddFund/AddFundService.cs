using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.AddFund;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.AddFund;
using BankUAPI.SharedKernel.Response.AddFund;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace BankUAPI.Application.Implementation.AddFund
{
    public class AddFundService : IAddFundService
    {
        private readonly AllApiSettings _apiSettings;
        private readonly AppDbContext _db;
        private readonly ICommonRepositry commonRepositry;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddFundService(IOptions<AllApiSettings> apiSettings, ICommonRepositry CR, AppDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _apiSettings = apiSettings.Value;
            commonRepositry = CR;
            _httpClientFactory = httpClientFactory;
        }
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _userLocks = new();
        public async Task<LoginModel> Process(AddFundRequest obj)
        {
            int userId = Convert.ToInt32(obj.UserId);

            // 🔒 USER LEVEL LOCK
            var semaphore = _userLocks.GetOrAdd(userId, new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();

            try
            {
                var balanceDict = await commonRepositry.GetLatestBalancesAsync(userId);

                decimal currentBalance = balanceDict.ContainsKey(userId)
                    ? balanceDict[userId]
                    : 0;
                // ❗ Validate Idempotency Key
                if (string.IsNullOrEmpty(obj.IdempotencyKey))
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Idempotency Key required"
                    };
                }

                // ✅ CHECK IF ALREADY PROCESSED
                var existingTxn = await _db.Addfunds
                    .FirstOrDefaultAsync(x =>
                        x.UserId == obj.UserId &&
                        x.IdempotencyKey == obj.IdempotencyKey);

                if (existingTxn != null)
                {
                    // 🔁 RETURN SAME RESPONSE (NO DUPLICATE HIT)
                    return new LoginModel
                    {
                        Status = "SUCCESS",
                        Message = "Already processed",
                        Data = new AddFundResult
                        {
                            Transactions = new List<AddFundResponse>
                    {
                        new AddFundResponse
                        {
                            MobileNo = obj.Number,
                            Operator = "Add Fund",
                            Amount = existingTxn.Amount.ToString(),
                            Status = existingTxn.Status,
                            TxnID = existingTxn.OrderId,
                            TxnDate = existingTxn.ReqDate.ToString("yyyy-MM-dd HH:mm:ss")
                        }
                    }
                        }
                    };
                }

                if (obj.Apiversion != "1.0")
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Please update your app"
                    };
                }

                bool isValidUser = await IsUserValidAsync(obj.UserId);

                if (!isValidUser)
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Session Expired"
                    };
                }

                // ✅ Generate OrderId
                string transactionId = "ORD" + DateTime.Now.ToString("yyMMddHHmmssfff");

                obj.OrderId = transactionId;

                // 🚀 CALL API
                string content = await CreateAddFundOrderAsync(obj);

                if (string.IsNullOrWhiteSpace(content) || content == "-1")
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "API Error"
                    };
                }

                JObject json = JObject.Parse(content);

                var result = await HandleApiResponseAsync(json, obj, transactionId, content);

                if (!result.isSuccess)
                {
                    return new LoginModel
                    {
                        Status = "FAILED",
                        Message = result.message
                    };
                }

                return new LoginModel
                {
                    Status = "SUCCESS",
                    Message = "Payment initiated",
                    Data = new AddFundResult
                    {
                        Transactions = new List<AddFundResponse>
                {
                    new AddFundResponse
                    {
                        MobileNo = obj.Number,
                        Operator = "Add Fund",
                        Amount = obj.Amt,
                        Status = "PENDING",
                        TxnID = transactionId,
                        TxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                         CurrentBalance = currentBalance,
                        QRImageBase64 = result.data.QRImageBase64,
                        PaymentUrl = result.data.PaymentUrl,
                        TxnRefId = result.data.TxnRefId,
                        BHIM = result.data.BHIM,
                        PhonePe = result.data.PhonePe,
                        Paytm = result.data.Paytm,
                        GPay = result.data.GPay
                    }
                }
                    }
                };
            }
            catch (Exception ex)
            {
                return new LoginModel
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
            finally
            {
                semaphore.Release(); // 🔓 ALWAYS RELEASE
            }
        }

        public async Task<bool> IsUserValidAsync(string userId)
        {
            int uid = Convert.ToInt32(userId);

            return await _db.Registrations
                .AnyAsync(x => x.RegistrationId == uid);
        }

        public async Task<string> CreateAddFundOrderAsync(AddFundRequest req)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AddFundClient");

                var body = new
                {
                    token = _apiSettings.Token,
                    order_id = req.OrderId,
                    txn_amount = req.Amt,
                    txn_note = "Add Fund",
                    product_name = "Add Fund",
                    customer_name = req.Name,
                    customer_mobile = req.Number,
                    customer_email = req.Email,
                    redirect_url = req.RedirectUrl
                };

                var response = await client.PostAsJsonAsync("create", body);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"ERROR: {response.StatusCode} | {content}";
                }

                return content;
            }
            catch (Exception ex)
            {
                return $"EXCEPTION: {ex.Message}";
            }
        }

        public async Task<(bool isSuccess, string message, AddFundResponse data)>
  HandleApiResponseAsync(JObject json, AddFundRequest req, string transactionId, string rawResponse)
        {
            try
            {
                if (json["status"]?.ToString().ToUpper() == "TRUE")
                {
                    var results = json["results"];

                    var response = new AddFundResponse
                    {
                        QRImageBase64 = results["qr_image"]?.ToString(),
                        TxnRefId = results["txn_id"]?.ToString(),
                        PaymentUrl = results["payment_url"]?.ToString(),
                        BHIM = results["upi_intent"]?["bhim"]?.ToString(),
                        PhonePe = results["upi_intent"]?["phonepe"]?.ToString(),
                        Paytm = results["upi_intent"]?["paytm"]?.ToString(),
                        GPay = results["upi_intent"]?["gpay"]?.ToString()
                    };

                    var entity = new Addfund
                    {
                        UserId = req.UserId,
                        UserName = req.Name,
                        Amount = Convert.ToDecimal(req.Amt),
                        OrderId = transactionId,
                        TxnId = response.TxnRefId, 
                        Status = "Pending",
                        ReqDate = DateTime.Now,
                        Reqlogs = rawResponse,
                        IdempotencyKey = req.IdempotencyKey 
                    };

                    _db.Addfunds.Add(entity);
                    await _db.SaveChangesAsync();

                    return (true, "", response);
                }

                return (false, json["message"]?.ToString(), null);
            }
            catch
            {
                return (false, "Exception while processing response", null);
            }
        }
    }
}
