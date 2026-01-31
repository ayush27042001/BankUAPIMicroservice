using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class PaymentStatusEnquiryResponse
    {
        public PaymentTransactionStatusResp paymentTransactionStatusResp { get; set; } = default!;
    }

    public sealed class PaymentTransactionStatusResp
    {
        public MetaData metaData { get; set; } = default!;
        public PaymentStatusResourceData resourceData { get; set; } = default!;
    }

    public sealed class PaymentStatusResourceData
    {
        public string respCode { get; set; } = default!;
        public string status { get; set; } = default!;
        public string beneficiaryAccountNumber { get; set; } = default!;
        public string beneficiaryName { get; set; } = default!;
        public string errorId { get; set; } = default!;
        public string errorMessage { get; set; } = default!;
        public string transactionType { get; set; } = default!;
        public string paymentReferenceNumber { get; set; } = default!;
        public string transactionReferenceNumber { get; set; } = default!;
        public string transactionDate { get; set; } = default!;
    }
}
