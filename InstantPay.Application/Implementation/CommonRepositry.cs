using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
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
    }
}
