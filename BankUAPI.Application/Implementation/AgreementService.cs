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
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "UserId is required"
                    };
                }

                if (!long.TryParse(userId, out long userIdValue))
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "Invalid UserId"
                    };
                }

                // Check if user exists
                bool userExists = await _db.Registrations
                    .AnyAsync(x => x.RegistrationId == userIdValue, cn);

                if (!userExists)
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                var agreements = await _db.UserAgreements
                    .Where(x => x.UserId == userIdValue)
                    .Select(x => new AgreementData
                    {
                        Id = x.Id,
                        UserId = x.UserId.ToString(),
                        AgreementId = x.AgreementId,
                        AgreementType = x.AgreementType,
                        FilePath = $"{_AgreementSetting.BaseUrl.TrimEnd('/')}/{x.FilePath.Replace("~/", "").TrimStart('/')}",
                        Status = x.Status,
                        CreatedAt = Convert.ToDateTime(x.CreatedAt),
                        FullName = x.FullName
                    })
                    .ToListAsync(cn);

                if (!agreements.Any())
                {
                    return new AgreementResponse
                    {
                        Status = "ERR",
                        Message = "No agreement found for this user"
                    };
                }

                return new AgreementResponse
                {
                    Status = "SUCCESS",
                    Message = "Agreement fetched successfully",
                    Data = agreements
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
    }
}
