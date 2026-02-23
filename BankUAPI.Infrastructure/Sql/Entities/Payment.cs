using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class Payment
    {
        [Key]
        public long Id { get; set; }
        public string? TxnId { get; set; }
        public string? PayUMihPayId { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? Last4Card { get; set; }
        public string? BankRefNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
