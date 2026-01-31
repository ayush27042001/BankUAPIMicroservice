using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.IDFCPayout.Security
{
    public sealed class JwtSigner
    {
        private readonly RSA _rsa;
        private readonly IdfcBankOptions _options;

        public JwtSigner(IdfcBankOptions options)
        {
            _options = options;
            _rsa = RSA.Create();
        }

        public string CreateJwt()
        {
            
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string basePath = Path.Combine(webRootPath, "IDFCPEMFile");
            string filePath = Path.Combine(basePath, "private_key.pem");
            var pem = File.ReadAllText(filePath);
            _rsa.ImportFromPem(pem);
            var signingKey = new RsaSecurityKey(_rsa)
            {
                KeyId = _options.Kid
            };

            var signingCredentials = new SigningCredentials(
               signingKey,
               SecurityAlgorithms.RsaSha256);

            long expEpoch = DateTimeOffset.UtcNow
                .AddMinutes(15)
                .ToUnixTimeSeconds();

            string jti = Guid.NewGuid().ToString();
            var header = new JwtHeader(signingCredentials);
            header["kid"] = _options.Kid;

            var payload = new JwtPayload
            {
                { "jti", jti },
                { "sub", _options.ClientId },
                { "iss", _options.ClientId },
                { "aud", _options.Audience },
                { "exp", expEpoch }
            };

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
