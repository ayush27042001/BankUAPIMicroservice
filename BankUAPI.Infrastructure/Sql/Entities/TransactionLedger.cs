using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public sealed class TransactionLedger
    {
        [Key]
        public long Id { get; set; }
        public string? ClientCode { get; set; } = null!;
        public string? BankCode { get; set; } = "IDFC";
        public string? TransactionType { get; set; } = null!;
        public string? InternalTxnId { get; set; } = null!;
        public string? ExternalTxnId { get; set; }
        public string? DebitAccount { get; set; } = null!;
        public string? CreditAccount { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string? Status { get; set; } = null!;
        public string? BankResponseCode { get; set; }
        public string? BankResponseMessage { get; set; }
        public string? RequestJson { get; set; } = null!;
        public string? ResponseJson { get; set; } = null!;
        public DateTime? CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
