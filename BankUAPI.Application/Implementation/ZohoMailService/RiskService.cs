using BankUAPI.Application.Interface.ZohoMailService;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.ZohoMailSent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.ZohoMailService
{
    public class RiskService : IRiskService
    {
        private readonly AppDbContext _db;

        public RiskService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RiskResult> EvaluateRisk(string email, string deviceId, string ip)
        {
            int score = 0;

            var device = await _db.UserDevice
                .FirstOrDefaultAsync(x => x.Email == email && x.DeviceId == deviceId);

            if (device == null) score += 30;

            var ipExists = await _db.UserDevice
                .AnyAsync(x => x.Email == email && x.IpAddress == ip);
            
            if (!ipExists) score += 20;

            var otpCount = await _db.EmailOtp
                .Where(x => x.Email == email &&
                            x.CreatedAt >= DateTime.Now.AddMinutes(-10))
                .CountAsync();

            if (otpCount >= 3) score += 25;

            var lastOtp = await _db.EmailOtp
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (lastOtp != null && lastOtp.Attempts >= 3)
                score += 25;

            string decision = score switch
            {
                <= 30 => "Allow",
                <= 70 => "Challenge",
                _ => "Block"
            };

            return new RiskResult { Score = score, Decision = decision };
        }

        public async Task SaveTrustedDevice(string email, string deviceId, string ip)
        {
            var existing = await _db.UserDevice
                .FirstOrDefaultAsync(x => x.Email == email && x.DeviceId == deviceId);
            var emailData = await _db.Registrations.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefaultAsync();
            if (emailData == null)
            {
                return;
            }

            if (existing == null)
            {
                _db.UserDevice.Add(new UserDevices
                {
                    IsTrusted = true,
                    Email = email,
                    UserId = emailData.RegistrationId,
                    DeviceId = deviceId,
                    IpAddress = ip,
                    LastUsed = DateTime.Now
                });
            }
            else
            {
                existing.LastUsed = DateTime.Now;
                existing.IpAddress = ip;
            }

            await _db.SaveChangesAsync();
        }
    }
}
