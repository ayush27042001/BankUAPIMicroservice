using BankUAPI.Application.Interface;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public class ResetMpinService : IResetMpinService
    {
        private readonly AppDbContext _db;

        public ResetMpinService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ResetMpinResponse> ResetMpinAsync(ResetMpin model, CancellationToken cn)
        {
            try
            {
                if (model == null)
                {
                    return new ResetMpinResponse
                    {
                        Status = "ERR",
                        Message = "Invalid request"
                    };
                }
                if (model.NewMpin.Length != 4 || !model.NewMpin.All(char.IsDigit))
                {
                    return new ResetMpinResponse
                    {
                        Status = "ERR",
                        Message = "MPIN must be 4 digits number"
                    };
                }
                if (model.CurrentMpin == model.NewMpin)
                {
                    return new ResetMpinResponse
                    {
                        Status = "ERR",
                        Message = "New Mpin can not be same as Current Mpin"
                    };
                }
                int userId = Convert.ToInt32(model.UserId);

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == userId, cn);

                if (user == null)
                {
                    return new ResetMpinResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }
             
                if (user.Mpin != model.CurrentMpin)
                {
                    return new ResetMpinResponse
                    {
                        Status = "ERR",
                        Message = "Current MPIN is incorrect"
                    };
                }
             
                user.Mpin = model.NewMpin;

                _db.Registrations.Update(user);

                await _db.SaveChangesAsync(cn);

                return new ResetMpinResponse
                {
                    Status = "SUCCESS",
                    Message = "MPIN updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResetMpinResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}
