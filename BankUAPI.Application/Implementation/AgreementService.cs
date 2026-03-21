using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public class AgreementService : IAgreementService
    {
        private readonly AppDbContext _db;
        private readonly AgreementSetting _AgreementSetting;

        public AgreementService(
            AppDbContext db,
            IOptions<AgreementSetting> AgreementSetting)
        {
            _db = db;
            _AgreementSetting = AgreementSetting.Value;
        }

        public async Task<AgreementResponse> GetAgreementAsync(string userId, CancellationToken cn)
        {
            try
            {
                if (!long.TryParse(userId, out long uid))
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "Invalid UserId"
                    };
                }

                var userExists = await _db.Registrations
                    .AnyAsync(x => x.RegistrationId == uid, cn);

                if (!userExists)
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                string baseUrl = _AgreementSetting.BaseUrl.TrimEnd('/');

                var rawData = await _db.UserAgreements
                    .Where(x => x.UserId == uid)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync(cn);

                var data = rawData.Select(x => new AgreementData
                {
                    Id = x.Id,
                    UserId = x.UserId.ToString(),
                    AgreementId = x.AgreementId,
                    AgreementType = x.AgreementType ?? "",

                    FilePath = GetFullUrl(x.FilePath, baseUrl),
                    UserSignedPath = GetFullUrl(x.UserSignedPath, baseUrl),
                    AdminSignedPath = GetFullUrl(x.AdminSignedPath, baseUrl),

                    Status = x.Status ?? "",
                    FullName = x.FullName ?? "",
                    CreatedAt = x.CreatedAt ?? DateTime.MinValue
                }).ToList();
                    

                return new AgreementResponse
                {
                    Status = data.Any() ? "SUCCESS" : "ERR",
                    Message = data.Any() ? "Agreement fetched successfully" : "No agreement found",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new AgreementResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }

        }
        private string GetFullUrl(string path, string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";

            path = path.Replace("~/", "").Trim();

            if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return path;

            return $"{baseUrl}/{path.TrimStart('/')}";
        }
    }
}
