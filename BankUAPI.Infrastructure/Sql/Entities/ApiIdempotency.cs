using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public sealed class ApiIdempotency
    {
        [Key]
        public long Id { get; set; }
        public string IdempotencyKey { get; set; } = null!;
        public string ClientCode { get; set; } = null!;
        public string RequestHash { get; set; } = null!;
        public string ResponseJson { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
