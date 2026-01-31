using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class FundTransferResponse
    {
        public InitiateAuthGenericFundTransferAPIResp initiateAuthGenericFundTransferAPIResp { get; set; } = new();

        public sealed class InitiateAuthGenericFundTransferAPIResp
        {
            public MetaData metaData { get; set; } = new();
            public ResourceData resourceData { get; set; } = new();
        }

        public sealed class MetaData
        {
            public string status { get; set; } = default!;
            public string message { get; set; } = default!;
            public string version { get; set; } = default!;
            public DateTime time { get; set; }
        }

        public sealed class ResourceData
        {
            public string status { get; set; } = default!; 
            public string transactionID { get; set; } = default!;
            public string transactionReferenceNo { get; set; } = default!;
            public string beneficiaryName { get; set; } = default!;
        }
    }

}
