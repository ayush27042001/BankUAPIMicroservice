using BankUAPI.Application.Interface.AddFund;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.AddFund
{
    public class AddFundClient : IAddFundClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AddFundClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<string> CheckStatusAsync(object payload, CancellationToken ct)
        {
            string baseUrl = _config["AllApi:BaseUrl"];
            string token = _config["AllApi:Token"];

            var body = new
            {
                token = token,
                order_id = payload.GetType().GetProperty("order_id")?.GetValue(payload)
            };

            var response = await _http.PostAsJsonAsync($"{baseUrl}/status", body, ct);

            return await response.Content.ReadAsStringAsync(ct);
        }

        public async Task<string> CreateOrderAsync(object payload, CancellationToken ct)
        {
            string baseUrl = _config["AllApi:BaseUrl"];
            string token = _config["AllApi:Token"];

            var body = new
            {
                token = token,
                payload
            };

            var response = await _http.PostAsJsonAsync($"{baseUrl}/create", body, ct);

            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}
