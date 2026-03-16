using BankUAPI.Application.Interface.BankAccount;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.Bank_Account;
using BankUAPI.SharedKernel.Response.Bank_Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Bank_Account
{
    public class BankAccountService : IBankAccountService
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CashfreeSetting _cashfreeSetting;

        public BankAccountService(AppDbContext db, IHttpClientFactory httpClientFactory, IOptions<CashfreeSetting> CashfreeSetting)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
            _cashfreeSetting = CashfreeSetting.Value;
        }

        public async Task<BankAccountResponse> AddBankAccountAsync(AddBankAccountRequest request, CancellationToken cn)
        {
            try
            {
                if (!long.TryParse(request.UserId, out long userId))
                {
                    return new BankAccountResponse
                    {
                        Status = "ERR",
                        Message = "Invalid UserId"
                    };
                }

                var user = await _db.Registrations
                    .FirstOrDefaultAsync(x => x.RegistrationId == userId, cn);

                if (user == null)
                {
                    return new BankAccountResponse
                    {
                        Status = "ERR",
                        Message = "User not found"
                    };
                }

                string userName = user.FullName.ToUpper();

                var client = _httpClientFactory.CreateClient();

                var body = new
                {
                    bank_account = request.AccountNo,
                    ifsc = request.IFSC,
                    name = user.FullName,
                    phone = request.Phone
                };

                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{_cashfreeSetting.BaseUrl}verification/bank-account/sync"
                );

                httpRequest.Headers.Add("x-client-id", _cashfreeSetting.ClientId);
                httpRequest.Headers.Add("x-client-secret", _cashfreeSetting.ClientSecret);
                httpRequest.Headers.Add("Accept", "application/json");

                httpRequest.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.SendAsync(httpRequest, cn);
                var apiResponse = await response.Content.ReadAsStringAsync(cn);

                var json = JObject.Parse(apiResponse);

                string status = json["account_status"]?.ToString();
                string statusCode = json["account_status_code"]?.ToString();
                string accHolder = CleanAccountHolderName(json["name_at_bank"]?.ToString());
                string bankName = json["bank_name"]?.ToString();

                if (status != "VALID")
                {
                    return new BankAccountResponse
                    {
                        Status = "ERR",
                        Message = statusCode
                    };
                }

                if (userName != accHolder)
                {
                    return new BankAccountResponse
                    {
                        Status = "ERR",
                        Message = "Enter only your own account details"
                    };
                }

                bool primaryBankExists = !string.IsNullOrEmpty(user.BankAccount);

                if (!primaryBankExists)
                {
   
                    user.BankAccount = request.AccountNo;
                    user.Ifsc = request.IFSC;
                    user.AccHolder = accHolder;
                    user.BankName = bankName;

                    _db.Registrations.Update(user);
                    await _db.SaveChangesAsync(cn);

                    return new BankAccountResponse
                    {
                        Status = "SUCCESS",
                        Message = "Primary bank account added successfully",
                        BankName = bankName,
                        AccountHolder = accHolder
                    };
                }
                else
                {

                    int bankCount = await _db.UserBanks.CountAsync(x => x.UserId == userId.ToString(), cn);

                    if (bankCount >= 5)
                    {
                        return new BankAccountResponse
                        {
                            Status = "ERR",
                            Message = "Maximum 5 bank accounts allowed"
                        };
                    }
                    var bank = new UserBanks
                    {
                        UserId = userId.ToString(),
                        AccountNo = request.AccountNo,
                        IFSC = request.IFSC,
                        BankName = bankName,
                        AccHolder = accHolder
                    };

                    await _db.UserBanks.AddAsync(bank, cn);
                    await _db.SaveChangesAsync(cn);

                    return new BankAccountResponse
                    {
                        Status = "SUCCESS",
                        Message = "Bank account added successfully",
                        BankName = bankName,
                        AccountHolder = accHolder
                    };
                }
            }
            catch (Exception ex)
            {
                return new BankAccountResponse
                {
                    Status = "ERR",
                    Message = ex.Message
                };
            }
        }
        private string CleanAccountHolderName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            name = name.ToUpper().Trim();

            string[] prefixes =
            {
                "MR ", "MRS ", "MS ", "SHRI ", "SMT ","MR. ", "MRS. ", "MS. ", "SHRI. ", "SMT. "
            };

            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix))
                {
                    name = name.Substring(prefix.Length);
                    break;
                }
            }

            // Remove relations / suffix part (everything after these)
            string[] relations =
            {
                " S/O ", " D/O ", " W/O ", " C/O "
            };

            foreach (var rel in relations)
            {
                int index = name.IndexOf(rel);
                if (index > -1)
                {
                    name = name.Substring(0, index);
                    break;
                }
            }

            // Remove trailing titles if any
            string[] suffixes =
            {
                " MR", " MRS", " MS", " SHRI", " SMT"
            };

            foreach (var suffix in suffixes)
            {
                if (name.EndsWith(suffix))
                {
                    name = name.Substring(0, name.Length - suffix.Length);
                    break;
                }
            }

            // Normalize spaces
            name = Regex.Replace(name, @"\s+", " ").Trim();

            return name;
        }
    }
}
