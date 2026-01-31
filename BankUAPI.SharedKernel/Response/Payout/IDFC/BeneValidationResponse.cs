using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class BeneValidationResponse
    {
        public BeneValidationResp beneValidationResp { get; set; } = default!;
    }

    public sealed class BeneValidationResp
    {
        public BeneMetaData metaData { get; set; } = default!;
        public BeneResourceData resourceData { get; set; } = default!;
    }

    public sealed class BeneMetaData
    {
        public string status { get; set; } = default!;
        public string message { get; set; } = default!;
        public string version { get; set; } = default!;
        public DateTime time { get; set; }
    }

    public sealed class BeneResourceData
    {
        public string creditorAccountId { get; set; } = default!;
        public string creditorName { get; set; } = default!;
        public string transactionReferenceNumber { get; set; } = default!;
        public string transactionId { get; set; } = default!;
        public string responseCode { get; set; } = default!;
        public DateTime transactionTime { get; set; }
        public string identifier { get; set; } = default!;
    }
}
