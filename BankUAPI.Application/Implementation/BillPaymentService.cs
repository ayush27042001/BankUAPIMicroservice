using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.BillPayement;
using BankUAPI.SharedKernel.Response.BillPayment;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public class BillPaymentService : IBillPaymentService
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;

        public BillPaymentService(AppDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BillPaymentResponse> GetBillers(BillPaymentRequest request, CancellationToken ct)
        {
            try
            {
                if (request == null || request.UserId <= 0)
                {
                    return new BillPaymentResponse
                    {
                        Status = "ERR",
                        Message = "Invalid UserId",
                        Billers = new List<BillerDto>()
                    };
                }

                long userId = request.UserId;

                var userExists = await _db.Registrations
                    .AnyAsync(x => x.RegistrationId == userId, ct);

                if (!userExists)
                {
                    return new BillPaymentResponse
                    {
                        Status = "ERR",
                        Message = "User not found",
                        Billers = new List<BillerDto>()
                    };
                }

                var billersData = await _db.INSBiller
                    .OrderBy(x => x.Id)
                    .ToListAsync(ct);

                var billers = billersData.Select(x => new BillerDto
                {
                    Id = x.Id,
                    CategoryKey = x.categoryKey,
                    CategoryName = x.categoryName,
                    IconUrl = x.iconUrl
                }).ToList();

                return new BillPaymentResponse
                {
                    Status = billers.Any() ? "SUCCESS" : "ERR",
                    Message = billers.Any() ? "Billers fetched successfully" : "No billers found",
                    Billers = billers
                };
            }
            catch (Exception ex)
            {
                return new BillPaymentResponse
                {
                    Status = "ERR",
                    Message = ex.Message,
                    Billers = new List<BillerDto>()
                };
            }
        }
        public async Task<OperatorResponse> GetOperators(OperatorRequest request, CancellationToken ct)
        {
            try
            {
                if (request == null || request.UserId <= 0 || string.IsNullOrEmpty(request.CategoryKey))
                {
                    return new OperatorResponse
                    {
                        Status = "ERR",
                        Message = "Invalid request",
                        Operators = new List<OperatorDto>()
                    };
                }

                long userId = request.UserId;

                var userExists = await _db.Registrations
                    .AnyAsync(x => x.RegistrationId == userId, ct);

                if (!userExists)
                {
                    return new OperatorResponse
                    {
                        Status = "ERR",
                        Message = "User not found",
                        Operators = new List<OperatorDto>()
                    };
                }

                var operatorData = await _db.INSOperator
                    .Where(x => x.categoryKey == request.CategoryKey)
                    .OrderBy(x => x.billerName)
                    .ToListAsync(ct);

                var operators = operatorData.Select(x => new OperatorDto
                {
                    BillerId = x.billerId,
                    BillerName = x.billerName,
                    CategoryKey = x.categoryKey,
                    CategoryName = x.categoryName,
                    IconUrl = x.iconUrl
                }).ToList();

                return new OperatorResponse
                {
                    Status = operators.Any() ? "SUCCESS" : "ERR",
                    Message = operators.Any() ? "Operators fetched successfully" : "No operators found",
                    Operators = operators
                };
            }
            catch (Exception ex)
            {
                return new OperatorResponse
                {
                    Status = "ERR",
                    Message = ex.Message,
                    Operators = new List<OperatorDto>()
                };
            }
        }
    }
}
