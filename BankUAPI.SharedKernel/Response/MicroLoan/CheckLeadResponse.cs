using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.MicroLoan
{
    public class CheckLeadResponse
    {
        public bool IsExisting { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationStatus { get; set; }
    }
    public class CreateLoanResponse
    {
        public string ApplicationId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class UploadDocsResponse
    {
        public string ApplicationId { get; set; }
        public string Status { get; set; }
        public List<DocumentInfo> Documents { get; set; }
    }

    public class DocumentInfo
    {
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
    public class LoanStatusResponse
    {
        public string ApplicationId { get; set; }
        public string? Status { get; set; }
        public string? Stage { get; set; }
        public DateTime? LastUpdated { get; set; }

        // 👇 Add full details
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? PAN { get; set; }
        public string? LoanAmount { get; set; }
        public string? LoanType { get; set; }

        // Documents
        public string? PanPath { get; set; }
        public string? GstPath { get; set; }
        public string? BankStatementPath { get; set; }
        public string? AadharPath { get; set; }
        public string? PhotoPath { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? DocumentUploadedOn { get; set; }
    }
    public class LoanTermsResponse
    {
        public string ApplicationId { get; set; }
        public decimal SanctionAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TenureMonths { get; set; }
        public decimal EMI { get; set; }
    }
    public class LoanDisbursalResponse
    {
        public string LoanId { get; set; }
        public string ApplicationId { get; set; }
        public decimal DisbursedAmount { get; set; }
        public DateTime DisbursedOn { get; set; }
        public string Status { get; set; }
    }
    public class LoanApplicationDetailResponse
    {
        public string ApplicationId { get; set; }
        public string ReUploadDocs { get; set; }
        public string RequirementMessage { get; set; }
    }
    public class ReUploadLoanDocResponse
    {
        public string ApplicationId { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
    }
    public class LoanApplicationListResponse
    {
        public string ApplicationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? ConfirmMobile { get; set; }
        public string? DOB { get; set; }
        public string? PAN { get; set; }
        public string? Gender { get; set; }
        public int? BusinessAge { get; set; }
        public decimal? MonthlyRevenue { get; set; }
        public string? LoanAmount { get; set; }
        public string? ApplicationStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DocumentUploadedOn { get; set; }

        public string? PanPath { get; set; }
        public string? GstPath { get; set; }
        public string? BankStatementPath { get; set; }
        public string? PhotoPath { get; set; }
        public string? AadharPath { get; set; }

        public string? RetailerId { get; set; }
        public string? RetailerName { get; set; }

        public string? LoanType { get; set; }

        public string? ReUploadDocs { get; set; }
        public string? RequirementMessage { get; set; }
    }
}
