using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public partial class LoanTermsSanction
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }

        public decimal? LoanAmount { get; set; }

        public decimal? DisbursalAmount { get; set; }

        public decimal? InterestRate { get; set; }

        public int? Tenure { get; set; }

        public decimal? EMI { get; set; }

        public decimal? GST { get; set; }

        public string? CalculationMethod { get; set; }

        public string? Period { get; set; }

        public string? TermsVersion { get; set; }

        public DateTime? GenerationDate { get; set; }
    }
}
