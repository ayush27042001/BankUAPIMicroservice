using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payout.IDFC
{
    public sealed class IdfcAccountBalanceResponse
    {
        public PrefetchAccountResp PrefetchAccountResp { get; init; } = default!;
    }

    public sealed class PrefetchAccountResp
    {
        public MetaData MetaData { get; init; } = default!;
        public ResourceData ResourceData { get; init; } = default!;
    }

    public sealed class MetaData
    {
        public string Status { get; init; } = default!;
        public string Message { get; init; } = default!;
        public string Version { get; init; } = default!;
        public DateTime Time { get; init; }
    }

    public sealed class ResourceData
    {
        public string AccountNumber { get; init; } = default!;
        public decimal AvailableBalance { get; init; }
        public string AccountStatus { get; init; } = default!;
        public string Freeze { get; init; } = default!;
    }
}
