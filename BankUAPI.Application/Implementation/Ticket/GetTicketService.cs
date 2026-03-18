using BankUAPI.Application.Interface.Ticket;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.KYC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharpCompress.Common;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Ticket
{
    public class GetTicketService : IGetTicketService
    {
        private readonly AppDbContext _db;
        private readonly AgreementSetting _agreementSetting;

        public GetTicketService(AppDbContext db, IOptions<AgreementSetting>  AgreementSetting)
        {
            _db = db;
            _agreementSetting = AgreementSetting.Value;
        }

        public async Task<GetTicketResponse> GetTicketsAsync(string userId, CancellationToken cn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new GetTicketResponse
                    {
                        Status = "ERR",
                        Message = "UserId is required"
                    };
                }

                if (!long.TryParse(userId, out long regId))
                {
                    return new GetTicketResponse
                    {
                        Status = "ERR",
                        Message = "Invalid RegistrationId",
                       
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == regId, cn);

                if (user == null)
                {
                    return new GetTicketResponse
                    {
                        Status = "ERR",
                        Message = "User not found",

                    };
                }
              
                var tickets = await _db.UserTickets
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreatedAt)
                   .Select(x => new TicketData
                   {
                       UserId = x.UserId ?? "",
                       TransactionId = x.TransactionId ?? "",
                       Type = x.Type ?? "",
                       Description = x.Description ?? "",
                       Status = x.Status ?? "",
                       CreatedAt = x.CreatedAt ?? DateTime.MinValue,
                       FilePath = string.IsNullOrEmpty(x.FilePath)
                        ? ""
                        : x.FilePath.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                            ? x.FilePath
                            : $"{_agreementSetting.BaseUrl.TrimEnd('/')}/{x.FilePath.TrimStart('/')}"
                   })
                    .ToListAsync(cn);

                if (!tickets.Any())
                {
                    return new GetTicketResponse
                    {
                        Status = "ERR",
                        Message = "No tickets found"
                    };
                }

                return new GetTicketResponse
                {
                    Status = "SUCCESS",
                    Message = "Tickets fetched successfully",
                    Tickets = tickets
                };
            }
            catch (Exception ex)
            {
                return new GetTicketResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}
