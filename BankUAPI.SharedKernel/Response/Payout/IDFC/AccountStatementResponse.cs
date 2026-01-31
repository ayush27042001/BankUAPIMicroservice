using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class AccountStatementResponse
    {
        public GetAccountStatementResp getAccountStatementResp { get; set; } = new();

        public sealed class GetAccountStatementResp
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
            public string accountNumber { get; set; } = default!;
            public List<StatementItem> collection { get; set; } = new();
        }

        public sealed class StatementItem
        {
            public string? description { get; set; }
            public string? amount { get; set; }
            public string? amountIndicator { get; set; }
            public string? balance { get; set; }
            public string? uuidNumber { get; set; }
            public string? postDate { get; set; }
            public string? postTime { get; set; }
        }
    }

}
