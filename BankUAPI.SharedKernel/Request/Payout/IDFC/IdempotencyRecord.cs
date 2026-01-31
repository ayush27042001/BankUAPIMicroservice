using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public sealed class IdempotencyRecord
    {
        public string Key { get; init; } = default!;
        public string RequestHash { get; init; } = default!;
        public string ResponseJson { get; init; } = default!;
        public DateTime CreatedAtUtc { get; init; }
    }
}
