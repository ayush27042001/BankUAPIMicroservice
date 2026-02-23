using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class PaymentGatewayLog
    {
        [Key]
        public int Id { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? ApiType { get; set; }
        public DateTime? ReqDate { get; set; }
        public string? StatusCode { get; set; }
        public string? Headers { get; set; }
        public string? Method { get; set; }
        public string? Url { get; set; }
        public string? ApiName { get; set; }
    }

}
