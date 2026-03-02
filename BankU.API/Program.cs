using BankUAPI.Application.Factory;
using BankUAPI.Application.Handler;
using BankUAPI.Application.IDFCPayout.Security;
using BankUAPI.Application.Implementation;
using BankUAPI.Application.Implementation.AddFund;
using BankUAPI.Application.Implementation.Commision.CommisionDistribution;
using BankUAPI.Application.Implementation.Commision.CommisionHeader;
using BankUAPI.Application.Implementation.Commision.CommissionPlans;
using BankUAPI.Application.Implementation.Commision.CommissionSlabs;
using BankUAPI.Application.Implementation.DMT.InstantPay;
using BankUAPI.Application.Implementation.Payment_Gateway;
using BankUAPI.Application.Implementation.Payouts.IDFC;
using BankUAPI.Application.Implementation.Payouts.IDFC.IDFCHttpClient;
using BankUAPI.Application.Implementation.Validator;
using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.AddFund;
using BankUAPI.Application.Interface.Commision.CommisionDistribution;
using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.Application.Interface.Commision.CommissionPlans;
using BankUAPI.Application.Interface.Commision.CommissionSlabs;
using BankUAPI.Application.Interface.DMT.Provider;
using BankUAPI.Application.Interface.Payment_Gateway.PayU;
using BankUAPI.Application.Interface.Payout.IDFCPayout;
using BankUAPI.Application.Interface.Validator;
using BankUAPI.Application.Middlewear;
using BankUAPI.Infrastructure.Mongo;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.AppSettingModel.IDFCPayout;
using BankUAPI.SharedKernel.AppSettingModel.PayU;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Net.Http.Headers;
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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
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
            "https://app.banku.co.in",
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

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });


builder.Services.AddTransient<InstantPayLoggingHandler>();

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
}).AddHttpMessageHandler<InstantPayLoggingHandler>();

builder.Services.AddHttpClient("AddFundClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AllApi:BaseUrl"]);     //Add Fund
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    client.Timeout = TimeSpan.FromSeconds(90);
});

builder.Services.Configure<AllApiSettings>(
    builder.Configuration.GetSection("AllApi")); 

builder.Services.AddHttpClient("IDFCClient", c =>
{
    c.DefaultRequestHeaders.Accept
        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient("PayUClient", client =>
{
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(90);
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
.AddHttpMessageHandler<PaymentGatewayLoggingHandler>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<InstantPayOptions>(
builder.Configuration.GetSection("InstantPay"));
builder.Services.Configure<IdfcBankOptions>(
builder.Configuration.GetSection("IdfcBank"));
builder.Services.Configure<PayUSettings>(
    builder.Configuration.GetSection("PayU"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<IdfcBankOptions>>().Value);
builder.Services.AddSingleton<JwtSigner>();
builder.Services.AddScoped<IIdfcAuthService, IdfcAuthService>();
builder.Services.AddScoped<IIdempotencyService, EfIdempotencyService>();
builder.Services.AddScoped<ApiLogger>();
builder.Services.AddScoped<TransactionLedgerService>();
builder.Services.AddScoped<IIdfcAccountService, IdfcAccountService>();
builder.Services.AddScoped<IIdfcBeneValidationService, IdfcBeneValidationService>();
builder.Services.AddScoped<IIdfcPaymentStatusService, IdfcPaymentStatusService>();
builder.Services.AddScoped<IAmountValidator, AmountValidator>();
builder.Services.AddScoped<IWalletValidator, WalletValidator>();
builder.Services.AddScoped<ICommissionProcessor, CommissionProcessor>();
builder.Services.AddScoped<IIDFCCommissionService, IDFCCommission>();
builder.Services.AddScoped<IRefundPolicy, IdfcRefundPolicy>();
builder.Services.AddScoped<IIdfcBankClient, IdfcBankClient>();
builder.Services.AddScoped<ICommonRepositry, CommonRepositry>();
builder.Services.AddScoped<IIdfcAccountStatementService, IdfcAccountStatementService>();
builder.Services.AddScoped<IIdfcFundTransferService,IdfcFundTransferService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInstantPayClient, InstantPayClient>();
builder.Services.AddScoped<IAepsCheckStatusService, AepsCheckStatusService>();
builder.Services.AddScoped<IAepsBapService, AepsBapService>();
builder.Services.AddScoped<IAepsBiometricKycService, AepsBiometricKycService>();
builder.Services.AddScoped<IAepsLoginService, AepsLoginService>();
builder.Services.AddScoped<IAepsSapService, AepsSapService>();
builder.Services.AddScoped<IAepsSignupService, AepsSignupService>();
builder.Services.AddScoped<ICommisionHeaderOps, CommisionHeaderOps>();
builder.Services.AddScoped<ICommisionPlanOps, CommisionPlanOps>();
builder.Services.AddScoped<ICommisionSlabsOps, CommisionSlabsOps>();
builder.Services.AddScoped<ICommissionDistributionService, CommissionDistributionService>();
builder.Services.AddScoped<IDmtCommissionService, DmtCommissionService>();
builder.Services.AddScoped<IDmtCommissionService, DmtCommissionService>();
builder.Services.AddScoped<InstantPayProvider>();
builder.Services.AddScoped<DmtProviderFactory>();
builder.Services.AddScoped<IPayUPaymentService, PayUPaymentService>();
builder.Services.AddTransient<PaymentGatewayLoggingHandler>();
builder.Services.AddScoped<IAddFundService, AddFundService>();//Add Fund Service By Sachin
builder.Services.AddScoped<IAddFundStatusService,AddFundStatusService>();//Add Fund Service By Sachin
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
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.Run();