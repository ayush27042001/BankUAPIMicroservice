using BankUAPI.Application.Interface.Recharge;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request.Recharge;
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
    public class FetchHlrService : IFetchHlrService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly PlanApi _PlanApi;
        public FetchHlrService(IHttpClientFactory httpClientFactory, IConfiguration config, AppDbContext db, IOptions<PlanApi> PlanApi)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _db = db;
            _PlanApi = PlanApi.Value;
        }

        public async Task<FetchHlrResponse> FetchDetails(FetchHlrRequest request, CancellationToken ct)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.MobileNo))
            {
                return new FetchHlrResponse
                {
                    Status = "ERR",
                    Message = "Invalid request"
                };
            }
            if (request.MobileNo.Length != 10 || !request.MobileNo.All(char.IsDigit))
            {
                return new FetchHlrResponse
                {
                    Status = "ERR",
                    Message = "Invalid mobile number"
                };
            }
            if (!int.TryParse(request.UserId, out int userId))
            {
                return new FetchHlrResponse
                {
                    Status = "ERR",
                    Message = "Invalid user id"
                };
            }

            bool isValidUser = await _db.Registrations
                .AnyAsync(x => x.RegistrationId == userId, ct);

            if (!isValidUser)
            {
                return new FetchHlrResponse
                {
                    Status = "ERR",
                    Message = "Invalid user"
                };
            }

            var client = _httpClientFactory.CreateClient();

            var url = $"{_PlanApi.BaseUrl}/api/Mobile/OperatorFetchNew" +
                      $"?ApiUserID={_PlanApi.UserId}" +
                      $"&ApiPassword={_PlanApi.Password}" +
                      $"&Mobileno={request.MobileNo}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            try
            {
                var response = await client.SendAsync(httpRequest, ct);
                var json = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    return new FetchHlrResponse
                    {
                        Status = "ERR",
                        Message = "Failed to fetch operator details"
                    };
                }

                var result = JsonSerializer.Deserialize<PlanApiHlrResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null || result.STATUS != "1" || result.ERROR == "1" || string.IsNullOrEmpty(result.Operator))
                {
                    return new FetchHlrResponse
                    {
                        Status = "ERR",
                        Message = result?.Message ?? "Failed to fetch details"
                    };
                }

                return new FetchHlrResponse
                {
                    Status = "SUCCESS",
                    Message = "Fetched successfully",
                    Mobile = result.Mobile,
                    Operator = result.Operator,
                    OperatorCode = result.OpCode,
                    Circle = result.Circle,
                    CircleCode = result.CircleCode
                };
            }
            catch (Exception ex)
            {
                return new FetchHlrResponse
                {
                    Status = "ERR",
                    Message = "Exception: " + ex.Message
                };
            }
        }
    }
}
