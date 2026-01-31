using BankUAPI.Infrastructure.Sql.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Payouts.IDFC
{
    public sealed class ApiLogger
    {
        private readonly AppDbContext _db;

        public ApiLogger(AppDbContext db)
            => _db = db;

        public async Task LogAsync(
            HttpRequestMessage req,
            HttpResponseMessage res,
            string requestBody,
            string responseBody,
            string apiName,
            string clientCode,
            long durationMs)
        {
            _db.ApiRequestResponseLogForIDFCPayout.Add(new ApiRequestResponseLogForIDFCPayout
            {
                ClientCode = clientCode,
                ApiName = apiName,
                HttpMethod = req.Method.Method,
                Url = req.RequestUri!.ToString(),
                RequestHeaders = JsonSerializer.Serialize(req.Headers),
                RequestBody = requestBody,
                ResponseHeaders = JsonSerializer.Serialize(res.Headers),
                ResponseBody = responseBody,
                HttpStatusCode = (int)res.StatusCode,
                DurationMs = (int)durationMs
            });

            await _db.SaveChangesAsync();
        }
    }

}
