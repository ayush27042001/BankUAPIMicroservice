using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.SharedKernel.Request.Payout.IDFC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class MemoryIdempotencyService 
    {
        private readonly IMemoryCache _cache;

        public MemoryIdempotencyService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<bool> ExistsAsync(string key, string requestHash)
        {
            if (_cache.TryGetValue<IdempotencyRecord>(key, out var record))
            {
                if (!string.Equals(record.RequestHash, requestHash,
                    StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        "Idempotency-Key reuse with different request payload");
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<string?> GetResponseAsync(string key)
        {
            if (_cache.TryGetValue<IdempotencyRecord>(key, out var record))
                return Task.FromResult<string?>(record.ResponseJson);

            return Task.FromResult<string?>(null);
        }

        public Task SaveAsync(
            string key,
            string requestHash,
            string responseJson)
        {
            var record = new IdempotencyRecord
            {
                Key = key,
                RequestHash = requestHash,
                ResponseJson = responseJson,
                CreatedAtUtc = DateTime.UtcNow
            };

            _cache.Set(
                key,
                record,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromHours(24)
                });

            return Task.CompletedTask;
        }
    }
}
