using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _db;
        private readonly AgreementSetting _AgreementSetting;

        public InvoiceService(AppDbContext db, IOptions<AgreementSetting> AgreementSetting)
        {
            _db = db;
            _AgreementSetting = AgreementSetting.Value;
        }

        public async Task<InvoiceResponse> GetInvoicesAsync(string userId, CancellationToken cn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new InvoiceResponse
                    {
                        Status = "ERR",
                        Message = "UserId is required"
                    };
                }

                if (!long.TryParse(userId, out long userIdValue))
                {
                    return new InvoiceResponse
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
                    return new InvoiceResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                var invoices = await _db.UserInvoices
                    .Where(x => x.UserId == userIdValue)
                    .Select(x => new InvoiceData
                    {
                        Id = x.Id,
                        UserId = x.UserId.ToString(),
                        InvoiceId = x.InvoiceId,
                        InvoiceType = x.InvoiceType,
                        StartDate = Convert.ToDateTime(x.StartDate),
                        EndDate = Convert.ToDateTime(x.EndDate),
                        FilePath = $"{_AgreementSetting.BaseUrl.TrimEnd('/')}/{x.FilePath.Replace("~/", "").TrimStart('/')}",
                        Status = x.Status,
                        CreatedAt = Convert.ToDateTime(x.CreatedAt)
                    })
                    .ToListAsync(cn);

                if (!invoices.Any())
                {
                    return new InvoiceResponse
                    {
                        Status = "ERR",
                        Message = "No invoices found for this user"
                    };
                }

                return new InvoiceResponse
                {
                    Status = "SUCCESS",
                    Message = "Invoices fetched successfully",
                    Data = invoices
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}
