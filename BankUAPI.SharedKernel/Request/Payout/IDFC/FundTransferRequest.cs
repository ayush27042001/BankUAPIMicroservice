using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public sealed class FundTransferRequest
    {
        public InitiateAuthGenericFundTransferAPIReq initiateAuthGenericFundTransferAPIReq { get; set; } = new();
        public sealed class InitiateAuthGenericFundTransferAPIReq
        {
            public string? tellerBranch { get; set; }
            public string? tellerID { get; set; }
            public string debitAccountNumber { get; set; } = default!;
            public string creditAccountNumber { get; set; } = default!;
            public string remitterName { get; set; } = default!;
            public string amount { get; set; } = default!;
            public string currency { get; set; } = "INR";
            public string transactionType { get; set; } = "IMPS";
            public string paymentDescription { get; set; } = default!;
            public string beneficiaryIFSC { get; set; } = default!;
            public string beneficiaryName { get; set; } = default!;
            public string? beneficiaryAddress { get; set; }
            public string? emailID { get; set; }
            public string? mobileNo { get; set; }
            public string? companyCode { get; set; } = "";
            public string? UserId { get; set; } = "";
        }
    }
}
