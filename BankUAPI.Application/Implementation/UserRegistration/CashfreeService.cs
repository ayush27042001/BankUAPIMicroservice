using BankUAPI.Application.Interface.UserRegistration;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.BankAccount;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.UserRegistration
{
    public class CashfreeService : ICashfreeService
    {
        private readonly IHttpClientFactory _http;
        private readonly CashfreeSetting _config;
        private readonly AppDbContext _db;
        public CashfreeService(IHttpClientFactory http, IOptions<CashfreeSetting> config, AppDbContext db)
        {
            _http = http;
            _config = config.Value;
            _db = db;
        }

        public async Task<PanVerifyResult> VerifyPan(string pan)
        {
            var client = _http.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_config.BaseUrl}verification/pan");

            request.Headers.Add("x-client-id", _config.ClientId);
            request.Headers.Add("x-client-secret", _config.ClientSecret);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new { pan }),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            await LogApiCall(pan, request.ToString(), response.ToString(),  "Pan_verify_App");
            var json = JObject.Parse(content);

            return new PanVerifyResult
            {
                IsValid = json["valid"]?.ToString() == "True",
                Name = json["registered_name"]?.ToString(),
                Type = json["type"]?.ToString()
            };
        }

        public async Task<PanVerifyResult> VerifyBusinessPan(string pan)
        {
            var result = await VerifyPan(pan);

            result.IsValid = (result.IsValid ?? false) &&
                     string.Equals(result.Type, "COMPANY", StringComparison.OrdinalIgnoreCase);

            return result;
        }

        public async Task<AadhaarOtpResult> SendAadhaarOtp(string aadhaar)
        {
            var client = _http.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_config.BaseUrl}verification/offline-aadhaar/otp");

            request.Headers.Add("x-client-id", _config.ClientId);
            request.Headers.Add("x-client-secret", _config.ClientSecret);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new { aadhaar_number = aadhaar }),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            await LogApiCall(aadhaar, request.ToString(), response.ToString(), "Aadhaar_Otp_App");
            var json = JObject.Parse(content);

            return new AadhaarOtpResult
            {
                Success = json["status"]?.ToString() == "SUCCESS",
                RefId = json["ref_id"]?.ToString(),
                Message = json["message"]?.ToString()
            };
        }

        public async Task<AadhaarVerifyResult> VerifyAadhaar(string otp, string refId, string panName)
        {
            var client = _http.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_config.BaseUrl}verification/offline-aadhaar/verify");

            request.Headers.Add("x-client-id", _config.ClientId);
            request.Headers.Add("x-client-secret", _config.ClientSecret);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new { otp, ref_id = refId }),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            await LogApiCall(panName, request.ToString(), response.ToString(), "Aadhaar_Verify_App");
            var json = JObject.Parse(content);

            string name = json["name"]?.ToString()?.ToUpper();

            if (json["status"]?.ToString() != "VALID" || name != panName.ToUpper())
            {
                return new AadhaarVerifyResult { Success = false };
            }

            return new AadhaarVerifyResult
            {
                Success = true,
                Name = name,
                Address = json["address"]?.ToString(),
                DOB = json["dob"]?.ToString(),
                Gender = json["gender"]?.ToString(),
                State = json["split_address"]?["state"]?.ToString(),
                Pincode = json["split_address"]?["pincode"]?.ToString()
            };
        }
        public async Task<GstVerifyResult> VerifyGst(string gst, string businessName)
        {
            var client = _http.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_config.BaseUrl}verification/gstin");

            request.Headers.Add("x-client-id", _config.ClientId);
            request.Headers.Add("x-client-secret", _config.ClientSecret);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    GSTIN = gst,
                    business_name = businessName
                }),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            await LogApiCall(gst, request.ToString(), response.ToString(), "GST_Verify_App");
            var json = JObject.Parse(content);

            return new GstVerifyResult
            {
                IsValid = json["valid"]?.ToString() == "True",
                LegalName = json["legal_name_of_business"]?.ToString(),
                TradeName = json["trade_name_of_business"]?.ToString()
            };
        }
        public async Task<CinVerifyResult> VerifyCin( string cin, string panName)
        {
            var client = _http.CreateClient();

            string externalRef = "UDYAM" + new Random().Next(100000, 999999);

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"{_config.BaseUrl}verification/cin");

            request.Headers.Add("x-client-id", _config.ClientId);
            request.Headers.Add("x-client-secret", _config.ClientSecret);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    verification_id = externalRef,
                    cin = cin
                }),
                Encoding.UTF8,
                "application/json");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            await LogApiCall(cin, request.ToString(), response.ToString(), "cin_Verify_App");
            var json = JObject.Parse(content);

            string status = json["status"]?.ToString()?.ToUpper();

            if (status != "VALID")
            {
                return new CinVerifyResult { IsValid = false };
            }

            bool directorMatched = false;

            var directors = json["director_details"] as JArray;

            if (directors != null)
            {
                foreach (var director in directors)
                {
                    string name = director["name"]?.ToString()?.ToUpper();

                    if (name == panName?.ToUpper())
                    {
                        directorMatched = true;
                        break;
                    }
                }
            }

            return new CinVerifyResult
            {
                IsValid = true,
                CompanyName = json["company_name"]?.ToString(),
                IsDirectorMatched = directorMatched
            };
        }

        public async Task LogApiCall(string userId, string request, string response, string apiType)
        {
            var log = new Apilog
            {
                UserId = userId,
                Request = request,
                Responce = response,
                ApiType = apiType,
                RequestDate = DateTime.Now
            };

            await _db.Apilogs.AddAsync(log);
            await _db.SaveChangesAsync();
        }
    }
}
