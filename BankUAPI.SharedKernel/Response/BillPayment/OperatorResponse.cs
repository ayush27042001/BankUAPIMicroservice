using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.BillPayment
{
    public class OperatorResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<OperatorDto> Operators { get; set; }
    }
    public class OperatorDto
    {
        public string BillerId { get; set; }
        public string BillerName { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryName { get; set; }
        public string IconUrl { get; set; }
    }
}
