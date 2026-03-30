using BankUAPI.Application.Interface.Recharge;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.Recharge;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.Recharge;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Recharge
{
    public class RechargePlanService : IRechargePlanService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly MPlan _MPlan;
        private readonly AppDbContext _db;

        public RechargePlanService(IHttpClientFactory httpClientFactory, AppDbContext db, IConfiguration config, IOptions<MPlan> MPlan)
        {
            _MPlan = MPlan.Value;
            _httpClientFactory = httpClientFactory;
            _db = db;
            _config = config;
        }

        public async Task<RechargePlanResponse> GetRechargePlans(RechargePlanRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return new RechargePlanResponse
                {
                    Status = "ERR",
                    Message = "Invalid request"
                };
            }

            int userId = Convert.ToInt32(request.UserId);

           
            bool isValidUser = await _db.Registrations
                .AnyAsync(x => x.RegistrationId == userId, ct);

            if (!isValidUser)
            {
                return new RechargePlanResponse
                {
                    Status = "ERR",
                    Message = "Invalid user"
                };
            }
            string operatorCode = await GetCode(request.OperatorCode, ct);
            string operatorName = await GetName(request.OperatorCode, ct);
            if (operatorCode== "Err")
            {
                return new RechargePlanResponse
                {
                    Status = "ERR",
                    Message = "Error Wrong Operator Code"
                };
            }
            var client = _httpClientFactory.CreateClient();

            var url = $"{_MPlan.BaseUrl}" +
                      $"?apikey={_MPlan.ApiKey}" +
                      $"&operator_code={operatorCode}" +
                      $"&circle_code={request.CircleCode}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(httpRequest, ct);
            var json = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                return new RechargePlanResponse
                {
                    Status = "ERR",
                    Message = "Failed to fetch recharge plans"
                };
            }

            var result = JsonSerializer.Deserialize<MplanApiResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null || result.Status != 1)
            {
                return new RechargePlanResponse
                {
                    Status = "ERR",
                    Message = "Invalid response from provider"
                };
            }
            return MapToRechargePlanResponse(result, operatorName, request.CircleCode);
        }

        public async Task<CircleResponse> GetCircles(CircleRequest request, CancellationToken ct)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserId))
            {
                return new CircleResponse
                {
                    Status = "ERR",
                    Message = "Invalid request"
                };
            }

            int userId = Convert.ToInt32(request.UserId);

            // ✅ Validate User
            bool isValidUser = await _db.Registrations
                .AnyAsync(x => x.RegistrationId == userId, ct);

            if (!isValidUser)
            {
                return new CircleResponse
                {
                    Status = "ERR",
                    Message = "Invalid user"
                };
            }

            // ✅ Fetch circles from DB
            var circles = await _db.tblcircle
                .Select(x => new CircleDetails
                {
                    CircleCode = x.CircleCode,
                    CircleName = x.CircleName,
                    INSPayCode = x.INSPAYCircle
                })
                .ToListAsync(ct);

            if (circles == null || !circles.Any())
            {
                return new CircleResponse
                {
                    Status = "ERR",
                    Message = "No circles found"
                };
            }

            return new CircleResponse
            {
                Status = "SUCCESS",
                Message = "Circles fetched successfully",
                Circles = circles
            };
        }
        private RechargePlanResponse MapToRechargePlanResponse(MplanApiResponse apiResponse, string Op, string Circle)
        {
            var response = new RechargePlanResponse
            {
                Status = "SUCCESS",
                Message = "Plans fetched successfully",
                Plans = new List<RechargePlanDetails>()
            };

            if (apiResponse.Records == null || !apiResponse.Records.Any())
                return response;

            foreach (var category in apiResponse.Records)
            {
                var categoryName = category.Key; 

                if (category.Value == null)
                    continue;

                foreach (var plan in category.Value)
                {
                    response.Plans.Add(new RechargePlanDetails
                    {
                        Amount = plan.Rs,
                        Description = plan.Desc,
                        Validity = plan.Validity,
                        Operator = Op,
                        Circle = Circle,

                      
                        Category = categoryName
                    });
                }
            }

            return response;
        }
        public async Task<string> GetCode(string operatorCode, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(operatorCode))
                return string.Empty;

            if (int.TryParse(operatorCode, out int opId))
            {
                var data = await _db.Tbloperators
                    .Where(x => x.Id == opId && x.Status == "Active")
                    .Select(x => x.MPlan)
                    .FirstOrDefaultAsync(ct);

                return data ?? string.Empty;
            }
            else
            {
                return "Err";
            }

        }
        public async Task<string> GetName(string operatorCode, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(operatorCode))
                return string.Empty;

            if (int.TryParse(operatorCode, out int opId))
            {
                var data = await _db.Tbloperators
                    .Where(x => x.Id == opId && x.Status == "Active")
                    .Select(x => x.OperatorName)
                    .FirstOrDefaultAsync(ct);

                return data ?? string.Empty;
            }
            else
            {
                return "Err";
            }

        }
    }
}
