using BankUAPI.Infrastructure.Sql.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Middlewear
{
    public class PaymentGatewayLoggingHandler : DelegatingHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentGatewayLoggingHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var log = new PaymentGatewayLog();
            log.ReqDate = DateTime.UtcNow;
            log.Method = request.Method.Method;
            log.Url = request.RequestUri.ToString();
            log.ApiType = "PAYU";
            log.ApiName = "PayU S2S";

            if (request.Content != null)
            {
                log.Request = await request.Content.ReadAsStringAsync();
            }
            log.Headers = string.Join(" | ",
                request.Headers.Select(h => $"{h.Key}:{string.Join(",", h.Value)}"));

            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content != null)
            {
                log.Response = await response.Content.ReadAsStringAsync();
            }

            log.StatusCode = ((int)response.StatusCode).ToString();
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.TblPaymentGatewayLogs.Add(log);
                await db.SaveChangesAsync();
            }

            return response;
        }
    }

}
