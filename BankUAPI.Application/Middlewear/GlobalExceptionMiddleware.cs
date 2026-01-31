using BankUAPI.Infrastructure.Sql.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Middlewear
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            try
            {
                context.Request.EnableBuffering();

                await _next(context);
            }
            catch (Exception ex)
            {
                var requestBody = await ReadRequestBody(context.Request);

                var log = new GlobalExceptionLog
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    QueryString = context.Request.QueryString.Value,
                    RequestBody = requestBody,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                };

                db.GlobalExceptionLogs.Add(log);
                await db.SaveChangesAsync();

                _logger.LogError(ex, "Unhandled exception occurred");
            }
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }

}
