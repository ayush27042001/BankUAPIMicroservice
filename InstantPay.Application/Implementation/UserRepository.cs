using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<UserOutlet?> GetOutletAsync(string userId, CancellationToken ct)
        {
            if (!int.TryParse(userId, out var registrationId))
                return Task.FromResult<UserOutlet?>(null);

            return _db.Registrations
                .Where(r => r.RegistrationId == registrationId)
                .Select(r => new UserOutlet
                {
                    UserId = r.RegistrationId.ToString(),
                    OutletId = r.OutletId
                })
                .FirstOrDefaultAsync(ct);
        }

        public Task<UserSignupProfile?> GetSignupProfileAsync(string userId, CancellationToken ct)
        {
            if (!int.TryParse(userId, out var registrationId))
                return Task.FromResult<UserSignupProfile?>(null);

            return _db.Registrations
                .Where(r => r.RegistrationId == registrationId)
                .Select(r => new UserSignupProfile
                {
                    UserId = r.RegistrationId.ToString(),
                    Name = r.FullName,
                    Email = r.Email
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task SaveOutletAsync(string userId, string outletId, CancellationToken ct)
        {
            var reg = await _db.Registrations
                .FirstAsync(x => x.RegistrationId == Convert.ToInt32(userId), ct);
            reg.OutletId = outletId;
            await _db.SaveChangesAsync(ct);
        }

        public async Task<string?> GetOutletIdAsync(int userId, CancellationToken ct)
        {
            return await _db.Registrations
                .AsNoTracking()
                .Where(x => x.RegistrationId == userId)
                .Select(x => x.OutletId)
                .FirstOrDefaultAsync(ct);
        }



    }
}
