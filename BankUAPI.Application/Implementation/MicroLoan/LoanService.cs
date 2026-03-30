using BankUAPI.Application.Interface.MicroLoan;
using BankUAPI.Application.Interface.UserRegistration;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel.AddFund;
using BankUAPI.SharedKernel.Request.MicroLoan;
using BankUAPI.SharedKernel.Response.BankAccount;
using BankUAPI.SharedKernel.Response.MicroLoan;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.MicroLoan
{
    public class LoanService : ILoanService
    {
        private readonly AppDbContext _db;
        private readonly TicketSetting _TicketSetting;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICashfreeService _cashfreeService;

        public LoanService(
             IOptions<TicketSetting> TicketSetting,
            AppDbContext db,
            IHttpClientFactory httpClientFactory,
            ICashfreeService cashfreeService)
        {
            _db = db;
            _TicketSetting = TicketSetting.Value;
            _httpClientFactory = httpClientFactory;
            _cashfreeService = cashfreeService;
        }

        // ================= CHECK LEAD =================
        public async Task<ApiResponse<CheckLeadResponse>> CheckLead(CheckLeadRequest request, CancellationToken cn)
        {
            var existing = await _db.LoanApplications
                .Where(x => x.Mobile == request.Mobile && x.Email == request.Email)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(cn);

            if (existing == null)
                return Success(new CheckLeadResponse { IsExisting = false }, "New user");

            if (existing.RetailerId != request.UserId)
                return Error<CheckLeadResponse>("Please contact your retailer");

            return Success(new CheckLeadResponse
            {
                IsExisting = true,
                ApplicationId = existing.ApplicationId.ToString(),
                ApplicationStatus = existing.ApplicationStatus
            }, "Existing application found");
        }

        // ================= CREATE =================
        public async Task<ApiResponse<CreateLoanResponse>> CreateApplication(CreateLoanRequest request, CancellationToken cn)
        {
            try
            {
                // 🔥 PAN VERIFY USING CASHFREE SERVICE
                var panResult = await _cashfreeService.VerifyPan(request.PAN);

                if (panResult == null || panResult.IsValid != true)
                    return Error<CreateLoanResponse>("PAN verification failed");

                string panName = panResult.Name?.ToUpper() ?? "";

                string firstName = request.FirstName?.ToUpper() ?? "";
                string lastName = request.LastName?.ToUpper() ?? "";

                if (!panName.Contains(firstName) && !panName.Contains(lastName))
                    return Error<CreateLoanResponse>("PAN name mismatch");

                // 🔁 Duplicate PAN check
                bool exists = await _db.LoanApplications
                    .AnyAsync(x => x.PAN == request.PAN, cn);

                if (exists)
                    return Error<CreateLoanResponse>("PAN already exists");

                // 💾 Save Application
                var entity = new LoanApplications
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Mobile = request.Mobile,
                    ConfirmMobile= request.Mobile,
                    Email = request.Email,
                    PAN = request.PAN,
                    Gender = request.Gender,
                    BusinessAge = request.BusinessAge,
                    LoanType = request.LoanType,
                    MonthlyRevenue = request.MonthlyRevenue,
                    LoanAmount = request.LoanAmount,
                    RetailerId = request.UserId,
                    ApplicationStatus = "Draft",
                    CreatedOn = DateTime.UtcNow
                };

                await _db.LoanApplications.AddAsync(entity, cn);
                await _db.SaveChangesAsync(cn);

                return Success(new CreateLoanResponse
                {
                    ApplicationId = entity.ApplicationId.ToString(),
                    Status = entity.ApplicationStatus,
                    CreatedOn = Convert.ToDateTime(entity.CreatedOn)
                }, "Application created");
            }
            catch (Exception ex)
            {
                return Error<CreateLoanResponse>(ex.Message);
            }
        }

        // ================= UPLOAD =================
        public async Task<ApiResponse<string>> UploadDocuments(UploadLoanDocsRequest request, CancellationToken cn)
        {
            if (!int.TryParse(request.ApplicationId, out int appId))
                return Error<string>("Invalid ApplicationId");

            var app = await _db.LoanApplications.FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

            if (app == null)
                return Error<string>("Application not found");
            if (!IsValidPdf(request.Pan))
                return Error<string>("PAN document must be a PDF");

            if (!IsValidPdf(request.Bank))
                return Error<string>("Bank statement must be a PDF");

            if (!IsValidPdf(request.Aadhar))
                return Error<string>("Aadhar must be a PDF");

            if (!IsValidPdf(request.Photo))
                return Error<string>("Photo must be a PDF");

            if (request.Gst != null && !IsValidPdf(request.Gst))
                return Error<string>("GST must be a PDF");
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/LoanDocs");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            async Task<string?> SaveFile(IFormFile? file)
            {
                if (file == null || file.Length == 0)
                    return null;

                string ext = Path.GetExtension(file.FileName).ToLower();

                if (ext != ".pdf")
                    throw new Exception("Only PDF files are allowed");

                string fileName = Guid.NewGuid() + ext;
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream, cn);

                return $"{_TicketSetting.BaseUrl.TrimEnd('/')}/LoanDocs/{fileName}";
            }
            app.PanPath = await SaveFile(request.Pan);
            if (request.Gst != null && request.Gst.Length > 0)
            {
                app.GstPath = await SaveFile(request.Gst);
            }
            app.BankStatementPath = await SaveFile(request.Bank);
            app.AadharPath = await SaveFile(request.Aadhar);
            app.PhotoPath = await SaveFile(request.Photo);

            app.ApplicationStatus = "Under Review";
            app.DocumentUploadedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync(cn);

            return Success("Documents uploaded successfully");
        }

        // ================= STATUS =================
        public async Task<ApiResponse<LoanStatusResponse>> GetLoanStatus(LoanStatusRequest request, CancellationToken cn)
        {
            int appId = Convert.ToInt32(request.ApplicationId);

            var app = await _db.LoanApplications.FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

            if (app == null)
                return Error<LoanStatusResponse>("Application not found");

            return Success(new LoanStatusResponse
            {
                ApplicationId = app.ApplicationId.ToString(),
                Status = app.ApplicationStatus,
                Stage = app.ApplicationStatus,
                LastUpdated = app.CreatedOn,
                FirstName = app.FirstName,
                LastName = app.LastName,
                Mobile = app.Mobile,
                Email = app.Email,
                PAN = app.PAN,
                LoanAmount = app.LoanAmount,
                LoanType = app.LoanType,

                PanPath = app.PanPath,
                GstPath = app.GstPath,
                BankStatementPath = app.BankStatementPath,
                AadharPath = app.AadharPath,
                PhotoPath = app.PhotoPath,

                CreatedOn = app.CreatedOn,
                DocumentUploadedOn = app.DocumentUploadedOn
            }, "Status fetched");
        }

        // ================= TERMS =================
        public async Task<ApiResponse<LoanTermsResponse>> GetLoanTerms(LoanTermsRequest request, CancellationToken cn)
        {
            int appId = Convert.ToInt32(request.ApplicationId);

            var terms = await _db.LoanTermsSanction
                .Where(x => x.ApplicationId == appId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cn);

            if (terms == null)
                return Error<LoanTermsResponse>("No terms found");

            return Success(new LoanTermsResponse
            {
                ApplicationId = appId.ToString(),
                SanctionAmount = Convert.ToDecimal(terms.LoanAmount),
                InterestRate = Convert.ToDecimal(terms.InterestRate),
                TenureMonths = Convert.ToInt32(terms.Tenure),
                EMI = Convert.ToDecimal(terms.EMI)
            }, "Terms fetched");
        }

        // ================= DISBURSAL =================
        public async Task<ApiResponse<LoanDisbursalResponse>> GetDisbursal(LoanDisbursalRequest request, CancellationToken cn)
        {
            int appId = Convert.ToInt32(request.ApplicationId);

            var app = await _db.LoanTermsSanction.FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);
            var app2 = await _db.LoanApplications.FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

            if (app == null)
                return Error<LoanDisbursalResponse>("Application not found");

            return Success(new LoanDisbursalResponse
            {
                LoanId = "BANKU" + appId,
                ApplicationId = appId.ToString(),
                DisbursedAmount = Convert.ToDecimal(app.DisbursalAmount),
                Status = app2.ApplicationStatus,
                DisbursedOn = DateTime.UtcNow
            }, "Disbursal fetched");
        }

        // ================= HELPERS =================
        private ApiResponse<T> Success<T>(T data, string msg = "Success")
        {
            return new ApiResponse<T> { Status = "SUCCESS", Message = msg, Data = data };
        }

        private ApiResponse<T> Error<T>(string msg)
        {
            return new ApiResponse<T> { Status = "ERR", Message = msg };
        }

        bool IsValidPdf(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return true; // optional file allowed

            return Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }
    }
}
