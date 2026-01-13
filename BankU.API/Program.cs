using BankUAPI.Application.Factory;
using BankUAPI.Application.Implementation;
using BankUAPI.Application.Implementation.Commision.CommisionDistribution;
using BankUAPI.Application.Implementation.Commision.CommisionHeader;
using BankUAPI.Application.Implementation.Commision.CommissionSlabs;
using BankUAPI.Application.Implementation.DMT.InstantPay;
using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.Commision.CommisionDistribution;
using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.Application.Interface.Commision.CommissionSlabs;
using BankUAPI.Application.Interface.DMT.Provider;
using BankUAPI.Infrastructure.Mongo;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;


System.Net.ServicePointManager.Expect100Continue = false;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var configuration = builder.Configuration;
string dbProvider = configuration["DatabaseProvider"] ?? "Sql";
if (dbProvider == "Mongo")
{
    builder.Services.Configure<MongoDbSettings>(configuration.GetSection("ConnectionStrings:Mongo"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("Sql")));
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banku API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllFrontends", policy =>
    {
        policy.WithOrigins(
            "https://demo2.instantpayment.co.in",
            "https://neqs.co.in",
            "http://localhost:54956"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // required for auth cookies/tokens
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });



builder.Services.AddHttpClient("InsPay", client =>
{
    client.Timeout = TimeSpan.FromSeconds(20);
    client.DefaultRequestHeaders.ExpectContinue = false;
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new SocketsHttpHandler
    {
        Expect100ContinueTimeout = TimeSpan.Zero,
        ConnectTimeout = TimeSpan.FromSeconds(5),

        // 🔥 FORCE IPv4
        ConnectCallback = async (context, ct) =>
        {
            var addresses = await Dns.GetHostAddressesAsync(context.DnsEndPoint.Host);
            var ipv4 = addresses.First(a => a.AddressFamily == AddressFamily.InterNetwork);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(ipv4, context.DnsEndPoint.Port, ct);
            return new NetworkStream(socket, ownsSocket: true);
        },

        UseProxy = false,
        AutomaticDecompression = DecompressionMethods.None
    };
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<InstantPayOptions>(
builder.Configuration.GetSection("InstantPay"));
builder.Services.AddScoped<ICommonRepositry, CommonRepositry>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInstantPayClient, InstantPayClient>();
builder.Services.AddScoped<IAepsCheckStatusService, AepsCheckStatusService>();
builder.Services.AddScoped<IAepsBapService, AepsBapService>();
builder.Services.AddScoped<IAepsBiometricKycService, AepsBiometricKycService>();
builder.Services.AddScoped<IAepsLoginService, AepsLoginService>();
builder.Services.AddScoped<IAepsSapService, AepsSapService>();
builder.Services.AddScoped<IAepsSignupService, AepsSignupService>();
builder.Services.AddScoped<ICommisionHeaderOps, CommisionHeaderOps>();
builder.Services.AddScoped<ICommisionSlabsOps, CommisionSlabsOps>();
builder.Services.AddScoped<ICommissionDistributionService, CommissionDistributionService>();
builder.Services.AddScoped<InstantPayProvider>();
builder.Services.AddScoped<DmtProviderFactory>();
var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banku API v1");
    });
}
//app.UseHttpsRedirection();
app.UseCors("AllowAllFrontends");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.Run();