using BankUAPI.Application.Interface.ZohoMailService;
using BankUAPI.SharedKernel.Request.ZohoMailSent;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.ZohoMailService
{
    public class ZohoEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ZohoEmailService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendOtpEmail(EmailRequest request)
        {
            var accessToken = await GetAccessTokenAsync();

            var htmlTemplate = GetOtpTemplate(request.Otp);

            var payload = new
            {
                fromAddress = _config["Zoho:Email"],
                toAddress = request.To,
                subject = request.Subject,
                content = htmlTemplate,
                mailFormat = "html"
            };

            var accountId = await GetAccountId(accessToken);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                $"https://mail.zoho.in/api/accounts/{accountId}/messages");

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);

            httpRequest.Content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Email failed: {error}");
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var url =
                $"https://accounts.zoho.in/oauth/v2/token?refresh_token={_config["Zoho:RefreshToken"]}&client_id={_config["Zoho:ClientId"]}&client_secret={_config["Zoho:ClientSecret"]}&grant_type=refresh_token";

            var response = await _httpClient.PostAsync(url, null);

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            return result.access_token;
        }

        private async Task<string> GetAccountId(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://mail.zoho.in/api/accounts");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(json);

            return data.data[0].accountId;
        }

        private string GetOtpTemplate(string otp)
        {
            return $@"
                <div style='margin:0; padding:0; background-color:#eef2f7; font-family:Segoe UI, Arial, sans-serif;'>
                <table width='100%' style='padding:20px 0; background:#eef2f7;'>
                <tr><td align='center'>
                <table width='440' style='background:#ffffff; border-radius:12px; overflow:hidden;'>

                <tr>
                <td align='center' style='padding:25px 20px 10px;'>
                <img src='https://www.banku.co.in/assets/images/org/1.png' style='max-width:140px;'/>
                </td>
                </tr>

                <tr>
                <td align='center'>
                <h2 style='color:#1e3a8a;'>Secure Verification</h2>
                </td>
                </tr>

                <tr>
                <td style='padding:25px 30px;'>
                <p>Dear Customer,</p>
                <p>Your OTP is valid for 5 minutes.</p>

                <div style='text-align:center; margin:20px;'>
                <span style='font-size:28px; font-weight:bold; letter-spacing:6px; color:#1e3a8a;'>
                {otp}
                </span>
                </div>

                <p style='font-size:12px;'>Do not share OTP.</p>
                </td>
                </tr>

                </table>
                </td></tr>
                </table>
                </div>";
        }
    }
}
