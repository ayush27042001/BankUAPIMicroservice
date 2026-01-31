using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class GlobalExceptionLog
    {
        [Key]
        public int Id { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? QueryString { get; set; }
        public string? RequestBody { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
