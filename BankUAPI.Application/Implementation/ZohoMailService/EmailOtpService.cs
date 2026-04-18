using BankUAPI.Application.Interface.ZohoMailService;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.ENUM;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.ZohoMailService
{
    public class EmailOtpService : IEmailOtpService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public EmailOtpService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        private string GetIp() =>
        _http.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        public async Task<bool> CanSendOtp(string email)
        {
            var last10Min = DateTime.Now.AddMinutes(-10);

            var count = await _db.EmailOtp
                .Where(x => x.Email == email && x.CreatedAt >= last10Min)
                .CountAsync();

            return count < 3;
        }


        public async Task<string> GenerateAndSaveOtp(string email, string deviceId)
        {
            var otp = GenerateOtp();

            var entity = new EmailOtp
            {
                CreatedAt= DateTime.Now,
                Attempts = 0,
                IsUsed = false,
                Email = email,
                Otp = otp,
                ExpiresAt = DateTime.Now.AddMinutes(5),
                DeviceId = deviceId,
                IpAddress = GetIp()
            };

            _db.EmailOtp.Add(entity);
            await _db.SaveChangesAsync();

            return otp;
        }

        public async Task<OtpVerifyStatus> VerifyOtp(string email, string otp, string deviceId)
        {
            var record = await _db.EmailOtp
                .Where(x => x.Email == email && x.IsUsed==false)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            if (record == null)
                return OtpVerifyStatus.NotFound;

            if (record.DeviceId != deviceId)
                return OtpVerifyStatus.DeviceMismatch;

            if (record.Attempts >= 5)
                return OtpVerifyStatus.Locked;
 
            if (record.ExpiresAt < DateTime.Now)
                return OtpVerifyStatus.Expired;

            if (record.Otp != otp)
            {
                record.Attempts++;

                if (record.Attempts >= 5)
                    record.IsUsed = true;

                await _db.SaveChangesAsync();
                return OtpVerifyStatus.InvalidOtp;
            }

            record.IsUsed = true;
            await _db.SaveChangesAsync();

            return OtpVerifyStatus.Success;
        }

        private string GenerateOtp()
        {
            var bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);
            int value = BitConverter.ToInt32(bytes, 0) & 0x7fffffff;
            return (value % 900000 + 100000).ToString();
        }
    }
}
