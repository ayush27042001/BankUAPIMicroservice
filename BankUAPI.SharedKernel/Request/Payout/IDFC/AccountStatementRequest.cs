using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public sealed class AccountStatementRequest
    {
        public GetAccountStatementReq getAccountStatementReq { get; set; } = new();
        public sealed class GetAccountStatementReq
        {
            public string? CBSTellerBranch { get; set; }
            public string? CBSTellerID { get; set; }
            public string fromDate { get; set; } = default!;
            public string toDate { get; set; } = default!;
            public string? numberOfTransactions { get; set; }
            public string? prompt { get; set; }
            public string? companyCode { get; set; } = "";
        }
    }
}
