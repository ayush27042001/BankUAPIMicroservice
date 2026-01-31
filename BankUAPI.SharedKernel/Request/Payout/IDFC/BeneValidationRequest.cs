using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public sealed class BeneValidationRequest
    {
        public BeneValidationReq beneValidationReq { get; set; } = default!;
    }

    public sealed class BeneValidationReq
    {
        public string? remitterName { get; set; } = default!;
        public string CompanyCode { get; set; } = default!;
        public string remitterMobileNumber { get; set; } = default!;
        public string debtorAccountId { get; set; } = default!;
        public string accountType { get; set; } = "10";
        public string creditorAccountId { get; set; } = default!;
        public string ifscCode { get; set; } = default!;
        public string paymentDescription { get; set; } = default!;
        public string transactionReferenceNumber { get; set; } = default!;
        public string merchantCode { get; set; } = default!;
        public string identifier { get; set; } = "Auto";
    }
}
