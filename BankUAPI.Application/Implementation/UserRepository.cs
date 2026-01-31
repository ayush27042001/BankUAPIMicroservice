using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request.Auth;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
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

        public Task<Registration?> GetUserData(string userId, CancellationToken ct)
        {
            if (!int.TryParse(userId, out var registrationId))
                return Task.FromResult<Registration?>(null);

            return _db.Registrations
                .Where(r => r.RegistrationId == registrationId)
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

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _db.Registrations.Where(id => id.MobileNo.Trim() == request.username.Trim() && id.AccountType == "BankU Seva Kendra").FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim("userid", user.RegistrationId.ToString()),
                new Claim("username", user.MobileNo ?? ""),
                new Claim("usertype", user.AccountType ?? "SuperAdmin"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new LoginResponseDto
            {
                Username = user.MobileNo ?? "",
                Usertype = user.AccountType ?? "",
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                messaege = "",
                userid = Convert.ToString(user.RegistrationId)
            };
        }
    }
}
