using BankUAPI.Application.Interface.BankAccount;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Bank_Account
{
    public class BankDetailsService: IBankDetails
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CashfreeSetting _cashfreeSetting;

        public BankDetailsService(AppDbContext db, IHttpClientFactory httpClientFactory, IOptions<CashfreeSetting> CashfreeSetting)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
            _cashfreeSetting = CashfreeSetting.Value;
        }
        public async Task<BankDetailsResponse> GetUserBanksAsync(GetBankRequest request, CancellationToken cn)
        {
            try
            {
                if (!long.TryParse(request.UserId, out long uid))
                {
                    return new BankDetailsResponse
                    {
                        Status = "ERR",
                        Message = "Invalid UserId"
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == uid, cn);

                if (user == null)
                {
                    return new BankDetailsResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                var banks = new List<BankDetail>();

                if (!string.IsNullOrEmpty(user.BankAccount))
                {
                    banks.Add(new BankDetail
                    {
                        AccountNo = user.BankAccount,
                        IFSC = user.Ifsc ?? "",
                        BankName = user.BankName ?? "",
                        AccountHolder = user.AccHolder ?? "",
                        Type = "Primary"
                    });
                }
                var userBanks = await _db.UserBanks  .Where(x => x.UserId == request.UserId)  .ToListAsync(cn);

                foreach (var bank in userBanks)
                {
                    banks.Add(new BankDetail
                    {
                        AccountNo = bank.AccountNo,
                        IFSC = bank.IFSC ?? "",
                        BankName = bank.BankName ?? "",
                        AccountHolder = bank.AccHolder ?? "",
                        Type = "Secondary"
                    });
                }

                return new BankDetailsResponse
                {
                    Status = "SUCCESS",
                    Message = "Bank accounts fetched successfully",
                    Banks = banks
                };
            }
            catch (Exception ex)
            {
                return new BankDetailsResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
    }
}
