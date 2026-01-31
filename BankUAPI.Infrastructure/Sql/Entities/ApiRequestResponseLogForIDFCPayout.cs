using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public sealed class ApiRequestResponseLogForIDFCPayout
    {
        [Key]
        public long Id { get; set; }
        public string? ClientCode { get; set; }
        public string ApiName { get; set; } = null!;
        public string HttpMethod { get; set; } = null!;
        public string Url { get; set; } = null!;

        public string RequestHeaders { get; set; } = null!;
        public string RequestBody { get; set; } = null!;

        public string ResponseHeaders { get; set; } = null!;
        public string ResponseBody { get; set; } = null!;

        public int HttpStatusCode { get; set; }
        public int DurationMs { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }

}
