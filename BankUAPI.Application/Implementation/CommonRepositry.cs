using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace BankUAPI.Application.Implementation
{
    public class CommonRepositry : ICommonRepositry
    {
        private readonly AppDbContext _db;
        private  IConfiguration _config;

        public CommonRepositry(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        public async Task InsertAepsTxnAsync( int userId, string transactionId, string mobile, decimal amount, string status, string prodKey, string prodName, string bankIin, string aadhaar, object apiResponse, string ipayUuid, string operatorId, string orderId, CancellationToken ct)
        {
            var userData = await (
                    from r in _db.Registrations.AsNoTracking()
                    join w in _db.Tbluserbalances.AsNoTracking()
                        on r.RegistrationId equals w.UserId
                    where r.RegistrationId == userId
                    orderby w.UserId descending
                    select new
                    {
                        UserName = r.FullName + "-" + r.MobileNo,
                        WalletBalance = w.NewBal
                    }
                ).FirstOrDefaultAsync(ct);

            if (userData == null)
                return;

            string maskedAadhaar = aadhaar.Length >= 4
                ? $"XXXX-XXXX-{aadhaar[^4..]}"
                : "XXXX";

            var txn = new TxnReport
            {
                Brid = operatorId ?? "",
                UserId =  userId.ToString(),
                UserName = userData.UserName,
                ServiceName = "AEPS",
                OperatorId = prodKey,
                OperatorName = prodName,
                MobileNo = mobile,
                AccountNo = maskedAadhaar,
                Amount = amount,
                OldBal = userData.WalletBalance,
                Commission = 0,
                Surcharge = 0,
                TotalCost = amount,
                NewBal = userData.WalletBalance,
                Apiname = "INSPAY",
                TxnDate = DateTime.UtcNow,
                TxnUpdateDate = DateTime.UtcNow,
                Status = status,
                ApiRequest = "",
                ApiResponse = JsonSerializer.Serialize(apiResponse),
                CallbackResponse = "",
                ApiMsg = orderId ?? "",
                IpAddress = "",
                TransactionId = transactionId
            };

            _db.TxnReports.Add(txn);
            await _db.SaveChangesAsync(ct);
        }

        public string AESEncryption(string data)
        {
            string EncryptionKey = _config.GetSection("InstantPay").GetValue<string?>("AesKey");
            string Iv = _config.GetSection("InstantPay").GetValue<string?>("Iv");
            string iniVector;
            byte[] IV = ASCIIEncoding.ASCII.GetBytes(Iv);
            byte[] clearBytes = Encoding.Unicode.GetBytes(data);
            AesCryptoServiceProvider crypt_provider;
            crypt_provider = new AesCryptoServiceProvider();
            crypt_provider.KeySize = 256;
            crypt_provider.Key = ASCIIEncoding.ASCII.GetBytes(EncryptionKey);
            crypt_provider.IV = IV;
            crypt_provider.Mode = CipherMode.CBC;
            crypt_provider.Padding = PaddingMode.PKCS7;
            ICryptoTransform transform = crypt_provider.CreateEncryptor();
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(data), 0, data.Length);
            byte[] encryptedData = new byte[encrypted_bytes.Length + IV.Length];
            IV.CopyTo(encryptedData, 0);
            encrypted_bytes.CopyTo(encryptedData, IV.Length);
            data = Convert.ToBase64String(encryptedData);
            return data;
        }

        public async Task InsertInsPayBankDetails(INSPayBankDetail request, CancellationToken ct)
        {
            if (request == null)
                return;
            var Data = _db.INSPayBankDetail.Where(id => id.bankId == request.bankId).FirstOrDefault();
            if (Data == null)
            {
                _db.INSPayBankDetail.Add(request);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                Data.bankId = request.bankId;
                Data.neftFailureRate = request.neftFailureRate;
                Data.impsFailureRate = request.impsFailureRate;
                Data.upiFailureRate = request.upiFailureRate;
                Data.ifscAlias = request.ifscAlias;
                Data.ifscGlobal = request.ifscGlobal;
                Data.impsEnabled = request.impsEnabled;
                Data.upiEnabled = request.upiEnabled;
                Data.neftEnabled = request.neftEnabled;
                _db.INSPayBankDetail.Update(Data);
                await _db.SaveChangesAsync(ct);
            }
        }

        public async Task<List<INSPayBankDetail>> FetchBankDetails()
        {
            var Data= await _db.INSPayBankDetail.ToListAsync();
            return Data;
        }

        public async Task AddDmtTxnReportAsync(Registration userData, string serviceName, string operatorId, string operatorName, string mobile, string accountNo, decimal txnAmount, decimal retailerCharge, string status, string apiName, object apiResponse, string transactionId, string? orderId, string? BankName, string? BeneName, string? Ifsccode, decimal? OldBalance, CancellationToken ct)
        {
            try
            {
                decimal? WalletBalance = await _db.Tbluserbalances.Where(id => id.UserId == userData.RegistrationId).OrderByDescending(id => id.Id).Select(u => u.NewBal).FirstOrDefaultAsync();
                var txn = new TxnReport
                {
                    Brid = operatorId ?? string.Empty,
                    UserId = userData.RegistrationId.ToString(),
                    UserName = userData.FullName + "-" + userData.MobileNo,
                    ServiceName = serviceName,
                    OperatorId = operatorId,
                    OperatorName = operatorName,
                    MobileNo = mobile,
                    AccountNo = accountNo,
                    Amount = txnAmount,
                    OldBal = OldBalance ?? WalletBalance,
                    Commission = 0m,
                    Surcharge = retailerCharge,
                    TotalCost = txnAmount + retailerCharge,
                    NewBal = status == "SUCCESS" || status == "PROCESS" ? OldBalance - (txnAmount + retailerCharge) : WalletBalance,
                    Apiname = apiName,
                    TxnDate = DateTime.Now,
                    TxnUpdateDate = DateTime.Now,
                    Status = status,
                    ApiRequest = string.Empty,
                    ApiResponse = JsonSerializer.Serialize(apiResponse),
                    CallbackResponse = string.Empty,
                    ApiMsg = orderId ?? string.Empty,
                    IpAddress = string.Empty,
                    BankName = BankName ?? string.Empty,
                    BeneName = BeneName ?? string.Empty,
                    IfscCode = Ifsccode ?? string.Empty,
                    TransactionId = transactionId
                };

                _db.TxnReports.Add(txn);
                await _db.SaveChangesAsync(ct);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WalletCheckResonse> WalletCheckValidationAsync( int userId, decimal txnAmount, CancellationToken ct = default)
        {
            var balance = await _db.Tbluserbalances
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.NewBal)
                .FirstOrDefaultAsync(ct);

            if (balance == null || balance <= 0)
            {
                return new WalletCheckResonse
                {
                    success = false,
                    message = "Invalid user or wallet balance not found"
                };
            }

            if (balance < txnAmount)
            {
                return new WalletCheckResonse
                {
                    success = false,
                    message = "Insufficient wallet balance"
                };
            }

            return new WalletCheckResonse
            {
                success = true,
                message = "Success"
            };
        }


        public async Task<WalletCheckResonse> RefundWalletBalance(int userId, decimal amount, string Remarks, CancellationToken ct = default)
        {
            if (amount <= 0)
            {
                return new WalletCheckResonse
                {
                    success = false,
                    message = "Invalid Amount"
                };
            }
            var balances = await GetLatestBalancesAsync(
                    userId
                );

            _db.Tbluserbalances.Add(new Tbluserbalance
            {
                UserId = userId,
                OldBal = balances[userId],
                Amount = amount,
                NewBal = balances[userId] + amount,
                CrDrType = "CR",
                TxnType = Remarks,
                Remarks = Remarks,
                TxnDatetime = DateTime.Now
            });

            _db.SaveChanges();
            return new WalletCheckResonse
            {
                success = true,
                message = "Refund Success"
            };
        }

        public async Task<Dictionary<int, decimal>> GetLatestBalancesAsync(
            params int?[] userIds)
        {
            var ids = userIds.Where(x => x.HasValue)
                             .Select(x => x!.Value)
                             .Distinct()
                             .ToList();

            return await _db.Tbluserbalances
                .Where(x => ids.Contains(x.UserId!.Value))
                .GroupBy(x => x.UserId!.Value)
                .Select(g => new
                {
                    UserId = g.Key,
                    Balance = g.OrderByDescending(x => x.Id).Select(x => x.NewBal ?? 0).First()
                }).ToDictionaryAsync(x => x.UserId, x => x.Balance);
        }


    }
}
