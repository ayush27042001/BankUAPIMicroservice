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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.AddFund
{
    public class AddFundStatusService : IAddFundStatusService
    {
        private readonly AllApiSettings _apiSettings;
        private readonly AppDbContext _db;
        private readonly ICommonRepositry _commonRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddFundStatusService(IOptions<AllApiSettings> apiSettings, ICommonRepositry commonRepository,AppDbContext db,IHttpClientFactory httpClientFactory)
        {
            _apiSettings = apiSettings.Value;
            _commonRepository = commonRepository;
            _db = db;
            _httpClientFactory = httpClientFactory;
        }
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _userLocks = new();
        public async Task<LoginModel> CheckStatus(StatusCheckRequest obj)
        {
            int userId = Convert.ToInt32(obj.UserId);

            // 🔒 USER LOCK
            var semaphore = _userLocks.GetOrAdd(userId, new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var balanceDict = await _commonRepository.GetLatestBalancesAsync(userId);

                decimal currentBalance = balanceDict.ContainsKey(userId)
                    ? balanceDict[userId]
                    : 0;
                if (string.IsNullOrEmpty(obj.UserId) || string.IsNullOrEmpty(obj.OrderId))
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Invalid request"
                    };
                }

                // ✅ Validate User
                if (!await IsUserValidAsync(userId))
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Session expired"
                    };
                }

                // 🔍 Fetch existing txn FIRST
                var existing = await _db.Addfunds
                    .FirstOrDefaultAsync(x => x.OrderId == obj.OrderId);

                if (existing == null)
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "Transaction not found"
                    };
                }

                // 🛑 DOUBLE CREDIT PROTECTION
                if (existing.Status?.ToUpper() == "SUCCESS")
                {
                    return new LoginModel
                    {
                        Status = "SUCCESS",
                        Message = "Already credited",
                        Data = new AddFundResult
                        {
                            Transactions = new List<AddFundResponse>
                            {
                                new AddFundResponse
                                {
                                    Amount = existing.AmountPaid.ToString(),
                                    Status = existing.Status,
                                    TxnID = existing.OrderId,
                                    TxnDate = existing.ReqDate.ToString("yyyy-MM-dd HH:mm:ss")
                                }
                            }
                        }
                    };
                }

                // 🚀 CALL API
                string apiResponse = await CallStatusAPIAsync(obj.OrderId);

                if (string.IsNullOrWhiteSpace(apiResponse))
                {
                    return new LoginModel
                    {
                        Status = "ERR",
                        Message = "API failed"
                    };
                }

                JObject json = JObject.Parse(apiResponse);

                var resultObj = json["results"];
                string paymentStatus = resultObj["status"]?.ToString()?.ToUpper();

                decimal amount = 0;
                decimal.TryParse(resultObj["txn_amount"]?.ToString(), out amount);

                // ✅ UPDATE DB
                existing.Status = paymentStatus == "SUCCESS" ? "Success" : "Failed";
                existing.AmountPaid = amount;
                existing.ApiResponse = apiResponse;

                _db.Addfunds.Update(existing);

                // 💰 CREDIT WALLET (ONLY ONCE)
                if (paymentStatus == "SUCCESS")
                {
                    await CreditWalletAsync(obj.UserId, amount);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new LoginModel
                {
                    Status = paymentStatus == "SUCCESS" ? "SUCCESS" : "FAILED",
                    Message = paymentStatus == "SUCCESS"
                        ? "Amount credited successfully"
                        : "Payment failed",
                    Data = new AddFundResult
                    {
                        Transactions = new List<AddFundResponse>
                        {
                            new AddFundResponse
                            {
                                Amount = amount.ToString(),
                                 CurrentBalance = currentBalance,
                                Status = paymentStatus,
                                TxnID = obj.OrderId,
                                TxnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new LoginModel
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
            finally
            {
                semaphore.Release();

                // 🧹 Cleanup lock (optional but recommended)
                if (semaphore.CurrentCount == 1)
                {
                    _userLocks.TryRemove(userId, out _);
                }
            }
        }

        // API CALL
        private async Task<string> CallStatusAPIAsync(string orderId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AddFundClient");

                var body = new
                {
                    token = _apiSettings.Token,
                    order_id = orderId
                };

                // 🔥 NO leading slash
                var response = await client.PostAsJsonAsync("status", body);
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

        // HANDLE RESPONSE
        private async Task<(bool isSuccess, string message)> HandleStatusResponseAsync( JObject json, StatusCheckRequest obj, string apiResponse)
        {
            try
            {
                string scode = json["status"]?.ToString()?.ToUpper();

                if (scode != "TRUE")
                {
                    return (false, json["message"]?.ToString() ?? "Status check failed");
                }

                var resultObj = json["results"];

                string paymentStatus = resultObj["status"]?.ToString()?.ToUpper();

                decimal amount = 0;
                decimal.TryParse(resultObj["txn_amount"]?.ToString(), out amount);

                // Check duplicate
                var existing = await _db.Addfunds
                    .FirstOrDefaultAsync(x => x.OrderId == obj.OrderId);

                if (existing != null && existing.Status.ToUpper() == "SUCCESS")
                {
                    return (true, "Already processed");
                }

                // Update AddFund Table
                if (existing != null)
                {
                    existing.Status = paymentStatus == "SUCCESS" ? "Success" : "Failed";
                    existing.AmountPaid = amount;
                    existing.ApiResponse = apiResponse;

                    _db.Addfunds.Update(existing);
                }

                // Credit Wallet if success
                if (paymentStatus == "SUCCESS")
                {
                    await CreditWalletAsync(obj.UserId, amount);
                    await _db.SaveChangesAsync();

                    return (true, "Amount credited successfully");
                }

                await _db.SaveChangesAsync();

                return (false, "Payment failed");
            }
            catch
            {
                return (false, "Error processing response");
            }
        }

        // CREDIT WALLET
        private async Task CreditWalletAsync(string userId, decimal amount)
        {
            int uid = Convert.ToInt32(userId);

            var balanceDict = await _commonRepository.GetLatestBalancesAsync(uid);
            decimal currentBalance = balanceDict.ContainsKey(uid) ? balanceDict[uid] : 0;

            decimal newBalance = currentBalance + amount;

            var walletEntry = new Tbluserbalance
            {
                OldBal = currentBalance,
                Amount = amount,
                NewBal = newBalance,
                TxnType = "Fund Added",
                CrDrType = "Credit",
                UserId = uid,
                Remarks = "Add Fund Success",
                TxnDatetime = DateTime.Now
            };

            _db.Tbluserbalances.Add(walletEntry);
        }

        // ✅ USER VALIDATION
        private async Task<bool> IsUserValidAsync(int userId)
        {
            return await _db.Registrations
                .AnyAsync(x => x.RegistrationId == userId);
        }
    }
}
