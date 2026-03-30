using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
   public partial class LoanApplications
{
        [Key]
        public int ApplicationId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string Mobile { get; set; } = null!;

    public string ConfirmMobile { get; set; } = null!;

    public string? DOB { get; set; }

    public string PAN { get; set; } = null!;

    public string? Gender { get; set; }

    public int BusinessAge { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public string? LoanAmount { get; set; }

    public string? ApplicationStatus { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? DocumentUploadedOn { get; set; }

    public string? PanPath { get; set; }

    public string? GstPath { get; set; }

    public string? BankStatementPath { get; set; }

    public string? PhotoPath { get; set; }

    public string? RetailerId { get; set; }

    public string? RetailerName { get; set; }

    public string? AadharPath { get; set; }

    public string? LoanType { get; set; }

    public string? ReUploadDocs { get; set; }

    public string? RequirementMessage { get; set; }
}
}
