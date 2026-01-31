using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public sealed class PaymentStatusEnquiryRequest
    {
        public PaymentTransactionStatusReq paymentTransactionStatusReq { get; set; } = default!;
    }

    public sealed class PaymentTransactionStatusReq
    {
        public string CompanyCode { get; set; } = "";
        public string tellerBranch { get; set; } = "";
        public string tellerID { get; set; } = "";
        public string transactionType { get; set; } = "IMPS";
        public string transactionReferenceNumber { get; set; } = default!;
        public string paymentReferenceNumber { get; set; } = default!;
        public string transactionDate { get; set; } = default!;
    }
}
