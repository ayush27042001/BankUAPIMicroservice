using BankUAPI.Application.IDFCPayout.Security;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class IdfcAuthService : IIdfcAuthService
    {
        private const string CACHE_KEY = "IDFC_ACCESS_TOKENMK";
        private readonly IHttpClientFactory _httpFactory;
        private readonly IMemoryCache _cache;
        private readonly JwtSigner _jwtSigner;
        private readonly IdfcBankOptions _options;
        private readonly string _scopeString;

        public IdfcAuthService(
            IHttpClientFactory httpFactory,
            IMemoryCache cache,
            JwtSigner jwtSigner,
            IdfcBankOptions options)
        {
            _httpFactory = httpFactory;
            _cache = cache;
            _jwtSigner = jwtSigner;
            _options = options;
            _scopeString = string.Join(' ', options.Scopes);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await _cache.GetOrCreateAsync(CACHE_KEY, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(14);

                var jwt = _jwtSigner.CreateJwt();

                var form = new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["scope"] = _scopeString,
                    ["client_id"] = _options.ClientId,
                    
                    ["client_assertion_type"] =
                        "urn:ietf:params:oauth:client-assertion-type:jwt-bearer",
                    ["client_assertion"] = jwt
                };

                var client = _httpFactory.CreateClient("IDFCClient");

                var response = await client.PostAsync(
                    _options.TokenUrl,
                    new FormUrlEncodedContent(form));


                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                return doc.RootElement
                          .GetProperty("access_token")
                          .GetString()!;
            })!;
        }
    }
}
