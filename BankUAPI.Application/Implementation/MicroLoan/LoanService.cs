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
        private readonly AgreementSetting _agreementSetting;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICashfreeService _cashfreeService;

        public LoanService( IOptions<TicketSetting> TicketSetting, AppDbContext db, IOptions<AgreementSetting> AgreementSetting,
            IHttpClientFactory httpClientFactory,  ICashfreeService cashfreeService)
        {
            _db = db;
            _TicketSetting = TicketSetting.Value;
            _agreementSetting = AgreementSetting.Value;
            _httpClientFactory = httpClientFactory;
            _cashfreeService = cashfreeService;
        }

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

        public async Task<ApiResponse<LoanApplicationDetailResponse>> GetApplicationDetails(LoanStatusRequest request,CancellationToken cn)
        {
            if (!int.TryParse(request.ApplicationId, out int appId))
                return Error<LoanApplicationDetailResponse>("Invalid ApplicationId");

            var app = await _db.LoanApplications
                .FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

            if (app == null)
                return Error<LoanApplicationDetailResponse>("Application not found");

            return Success(new LoanApplicationDetailResponse
            {
                ApplicationId = app.ApplicationId.ToString(),
                ReUploadDocs =string.IsNullOrEmpty(app.ReUploadDocs)
                        ? "" : app.ReUploadDocs.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? app.ReUploadDocs: $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.ReUploadDocs.TrimStart('~')}",
                RequirementMessage = app.RequirementMessage
            }, "Application details fetched");
        }

        public async Task<ApiResponse<ReUploadLoanDocResponse>> ReUploadDocument(ReUploadLoanDocRequest request, CancellationToken cn)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ApplicationId))
                    return Error<ReUploadLoanDocResponse>("ApplicationId is required");

                if (!int.TryParse(request.ApplicationId, out int appId))
                    return Error<ReUploadLoanDocResponse>("Invalid ApplicationId");

                var app = await _db.LoanApplications
                    .FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

                if (app == null)
                    return Error<ReUploadLoanDocResponse>("Application not found");

                if (request.File == null || request.File.Length == 0)
                    return Error<ReUploadLoanDocResponse>("File is required");

                string[] allowedExt = { ".pdf", ".jpg", ".jpeg", ".png" };

                string ext = Path.GetExtension(request.File.FileName).ToLower();

                if (!allowedExt.Contains(ext))
                    return Error<ReUploadLoanDocResponse>("Only PDF, JPG, JPEG, PNG files allowed");

                if (request.File.Length > 5 * 1024 * 1024)
                    return Error<ReUploadLoanDocResponse>("File size must be less than 5MB");

                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/LoanDocs");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = $"APP_{appId}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream, cn);
                }

                string fileUrl = $"{_TicketSetting.BaseUrl.TrimEnd('/')}/LoanDocs/{fileName}";

                app.ReUploadDocs = fileUrl;
                app.ApplicationStatus = "Re-Uploaded";
                app.DocumentUploadedOn = DateTime.UtcNow;

                await _db.SaveChangesAsync(cn);

                return Success(new ReUploadLoanDocResponse
                {
                    ApplicationId = appId.ToString(),
                    FilePath = fileUrl,
                    Status = "Re-Uploaded"
                }, "Document re-uploaded successfully");
            }
            catch (Exception ex)
            {
                return Error<ReUploadLoanDocResponse>(ex.Message);
            }
        }
        public async Task<ApiResponse<List<LoanApplicationListResponse>>> GetApplicationsByUser( LoanApplicationsByUserRequest request,CancellationToken cn)
        {
            var apps = await _db.LoanApplications
                .Where(x => x.RetailerId == request.UserId)
                .OrderByDescending(x => x.ApplicationId)
                .ToListAsync(cn);

            if (apps == null || apps.Count == 0)
                return Error<List<LoanApplicationListResponse>>("No applications found");

            var result = apps.Select(app => new LoanApplicationListResponse
            {
                ApplicationId = app.ApplicationId.ToString(),
                FirstName = app.FirstName,
                LastName = app.LastName,
                Email = app.Email,
                Mobile = app.Mobile,
                ConfirmMobile = app.ConfirmMobile,
                DOB = app.DOB,
                PAN = app.PAN,
                Gender = app.Gender,
                BusinessAge = app.BusinessAge,
                MonthlyRevenue = app.MonthlyRevenue,
                LoanAmount = app.LoanAmount,
                ApplicationStatus = app.ApplicationStatus,
                CreatedOn = app.CreatedOn,
                DocumentUploadedOn = app.DocumentUploadedOn,

                PanPath =  string.IsNullOrEmpty(app.PanPath) ? "" : app.PanPath.StartsWith("http")
                    ? app.PanPath : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.PanPath.TrimStart('~')}",
                GstPath =  string.IsNullOrEmpty(app.GstPath) ? "" : app.GstPath.StartsWith("http")
                    ? app.GstPath : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.GstPath.TrimStart('~')}",
                BankStatementPath = string.IsNullOrEmpty(app.BankStatementPath) ? "" : app.BankStatementPath.StartsWith("http")
                    ? app.BankStatementPath : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.BankStatementPath.TrimStart('~')}",
                PhotoPath = string.IsNullOrEmpty(app.PhotoPath) ? "" : app.PhotoPath.StartsWith("http")
                    ? app.PhotoPath : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.PhotoPath.TrimStart('~')}",
                AadharPath = string.IsNullOrEmpty(app.AadharPath) ? "" : app.AadharPath.StartsWith("http")
                    ? app.AadharPath : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.AadharPath.TrimStart('~')}",

                RetailerId = app.RetailerId,
                RetailerName = app.RetailerName,
                LoanType = app.LoanType,

                ReUploadDocs = string.IsNullOrEmpty(app.ReUploadDocs)  ? "" : app.ReUploadDocs.StartsWith("http")
                    ? app.ReUploadDocs : $"{_agreementSetting.BaseUrl.TrimEnd('/')}{app.ReUploadDocs.TrimStart('~')}",

                RequirementMessage = app.RequirementMessage
            }).ToList();

            return Success(result, "Applications fetched successfully");
        }

        public async Task<ApiResponse<string>> UpdateLoanApplication( UpdateLoanApplicationRequest request, CancellationToken cn)
        {
            try
            {
                if (!int.TryParse(request.ApplicationId, out int appId))
                    return Error<string>("Invalid ApplicationId");

                var app = await _db.LoanApplications
                    .FirstOrDefaultAsync(x => x.ApplicationId == appId, cn);

                if (app == null)
                    return Error<string>("Application not found");

                if (string.IsNullOrEmpty(request.FieldName))
                    return Error<string>("FieldName is required");

                string field = request.FieldName.Trim().ToLower();

                switch (field)
                {
                    case "firstname":
                        app.FirstName = request.Value;
                        break;

                    case "lastname":
                        app.LastName = request.Value;
                        break;

                    case "email":
                        app.Email = request.Value;
                        break;

                    case "mobile":
                        app.Mobile = request.Value;
                        break;

                    case "businessage":
                        app.BusinessAge = Convert.ToInt32(request.Value);
                        break;

                    case "loanamount":
                        if (!decimal.TryParse(request.Value, out decimal loanAmount))
                            return Error<string>("Invalid LoanAmount");

                        app.LoanAmount = request.Value;
                        break;

                    case "monthlyrevenue":
                        if (!decimal.TryParse(request.Value, out decimal revenue))
                            return Error<string>("Invalid MonthlyRevenue");

                        app.MonthlyRevenue = revenue;
                        break;

                    case "loantype":
                        app.LoanType = request.Value;
                        break;

                    case "gender":
                        app.Gender = request.Value;
                        break;

                    case "dob":
                        if (!DateTime.TryParse(request.Value, out DateTime dob))
                            return Error<string>("Invalid DOB");

                        app.DOB = request.Value;
                        break;

                    default:
                        return Error<string>("Invalid or restricted field");
                }

                await _db.SaveChangesAsync(cn);

                return Success("Application updated successfully");
            }
            catch (Exception ex)
            {
                return Error<string>(ex.Message);
            }
        }
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
