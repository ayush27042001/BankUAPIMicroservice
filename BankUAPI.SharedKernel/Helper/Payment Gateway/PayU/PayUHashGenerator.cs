using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Helper.Payment_Gateway.PayU
{
    public static class PayUHashGenerator
    {
        public static string GenerateHash(string key, string txnId, string amount, string productInfo, string firstName, string email, string salt)
        {
            string hashString = $"{key}|{txnId}|{amount}|{productInfo}|{firstName}|{email}|||||||||||{salt}";
            using var sha512 = SHA512.Create();
            byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(hashString));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
