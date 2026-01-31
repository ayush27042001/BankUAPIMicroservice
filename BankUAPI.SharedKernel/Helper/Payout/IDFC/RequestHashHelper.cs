using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Helper.Payout.IDFC
{
    public static class RequestHashHelper
    {
        public static string Compute(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
