using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.IDFCPayout.Crypto
{
    public static class AesCryptoService
    {
        public static string Encrypt(string plainText, string hexKey)
        {
            var key = Convert.FromHexString(hexKey);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var cipherBytes = encryptor.TransformFinalBlock(
                Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);

            return Convert.ToBase64String(aes.IV.Concat(cipherBytes).ToArray());
        }

        public static string Decrypt(string encryptedBase64, string hexKey)
        {
            var data = Convert.FromBase64String(encryptedBase64);
            var key = Convert.FromHexString(hexKey);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = data[..16];
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(
                data, 16, data.Length - 16);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
