using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.Ticket;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.AddFund;
using BankUAPI.SharedKernel.Request.Ticket;
using BankUAPI.SharedKernel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly TicketSetting _TicketSetting;
        private readonly AppDbContext _db;
        private readonly ICommonRepositry _commonRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public TicketService(
            IOptions<TicketSetting> TicketSetting,
            ICommonRepositry commonRepository,
            AppDbContext db,
            IHttpClientFactory httpClientFactory)
        {
            _TicketSetting = TicketSetting.Value;
            _commonRepository = commonRepository;
            _db = db;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TicketResponse> CreateTicketAsync(TicketModel model, IFormFile file, CancellationToken cn)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(cn);

            try
            {
                if (model == null)
                {
                    return new TicketResponse
                    {
                        Status = "ERR",
                        Message = "Invalid request"
                    };
                }

                int userId = Convert.ToInt32(model.UserId);

                // Validate User
                bool isValidUser = await _db.Registrations
                    .AnyAsync(x => x.RegistrationId == userId, cn);

                if (!isValidUser)
                {
                    return new TicketResponse
                    {
                        Status = "ERR",
                        Message = "Invalid user"
                    };
                }

                string filePath = null;

                // FILE UPLOAD
                if (file != null && file.Length > 0)
                {
                    string[] allowedExt = { ".jpg", ".jpeg", ".png", ".pdf" };

                    string ext = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExt.Contains(ext))
                    {
                        return new TicketResponse
                        {
                            Status = "ERR",
                            Message = "Only JPG, PNG or PDF files allowed"
                        };
                    }

                    // Folder path
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/Tickets");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    // Unique file name
                    string fileName = "Ticket_" + model.UserId + "_" + DateTime.Now.Ticks + ext;

                    string fullPath = Path.Combine(folder, fileName);

                    // Save file
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream, cn);
                    }

                   filePath = $"{_TicketSetting.BaseUrl.TrimEnd('/')}/Uploads/Tickets/{fileName}";
                }

                // Create Ticket Entity
                var ticket = new Infrastructure.Sql.Entities.UserTicket
                {
                    UserId = Convert.ToString(model.UserId),
                    TransactionId = model.TransactionId,
                    Type = model.Type,
                    Description = model.Description,
                    CreatedAt = DateTime.Now,
                    Status = "Pending",
                    FilePath = filePath
                };

                await _db.UserTickets.AddAsync(ticket, cn);

                await _db.SaveChangesAsync(cn);

                await transaction.CommitAsync(cn);

                return new TicketResponse
                {
                    Status = "SUCCESS",
                    Message = "Ticket created successfully",
                    FilePath = filePath

                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cn);

                return new TicketResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}