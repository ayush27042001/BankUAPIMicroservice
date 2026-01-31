using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Infrastructure.Sql.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class EfIdempotencyService : IIdempotencyService
    {
        private readonly AppDbContext _db;

        public EfIdempotencyService(AppDbContext db)
            => _db = db;

        public async Task<string?> GetExistingResponseAsync(
            string key, string requestHash, string clientCode)
        {
            var record = await _db.ApiIdempotency
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.IdempotencyKey == key &&
                    x.ClientCode == clientCode);

            if (record == null) return null;

            if (record.RequestHash != requestHash)
            {
                return "Idempotency key reused with different payload";
            }
            return record.ResponseJson;
        }

        public async Task SaveAsync(
            string key, string requestHash,
            string responseJson, string clientCode)
        {
            _db.ApiIdempotency.Add(new ApiIdempotency
            {
                IdempotencyKey = key,
                ClientCode = clientCode,
                RequestHash = requestHash,
                ResponseJson = responseJson
            });

            await _db.SaveChangesAsync();
        }
    }

}
