using BankUAPI.Infrastructure.Sql.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Handler
{
    public sealed class InstantPayLoggingHandler : DelegatingHandler
    {
        private readonly AppDbContext _dbContext;

        public InstantPayLoggingHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string requestBody = string.Empty;
            string responseBody = string.Empty;

            if (request.Content != null)
                requestBody = await request.Content.ReadAsStringAsync(cancellationToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.Content != null)
                responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            _dbContext.AepsrequestLogs.Add(new AepsrequestLog
            {
                ApiType = DetectApiType(request.RequestUri?.AbsolutePath),
                Reqdate = DateTime.Now,
                Url = request.RequestUri?.ToString(),
                Method = request.Method.Method,
                Headers = SerializeHeaders(request.Headers),
                Request = requestBody,
                Responce = responseBody,
                StatusCode = (int)response.StatusCode
            });
            await _dbContext.SaveChangesAsync(cancellationToken);
            return response;
        }

        private static string SerializeHeaders(HttpHeaders headers)
        {
            var safeHeaders = headers
                .Where(h =>
                    !h.Key.Contains("Secret", StringComparison.OrdinalIgnoreCase) &&
                    !h.Key.Contains("Auth", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(h => h.Key, h => string.Join(",", h.Value));

            return JsonSerializer.Serialize(safeHeaders);
        }

        private static string DetectApiType(string? path)
        {
            if (path == null) return "UNKNOWN";
            if (path.Contains("aeps", StringComparison.OrdinalIgnoreCase)) return "AEPS";
            if (path.Contains("remit", StringComparison.OrdinalIgnoreCase)) return "DMT";
            return "INSTANTPAY";
        }
    }

}
