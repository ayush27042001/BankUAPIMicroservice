using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public class InvoiceResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<InvoiceData> Data { get; set; }
    }

    public class InvoiceData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
