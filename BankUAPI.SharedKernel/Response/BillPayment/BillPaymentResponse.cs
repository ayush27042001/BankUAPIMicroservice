using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.BillPayment
{
    public class BillPaymentResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<BillerDto> Billers { get; set; }
    }
    public class BillerDto
    {
        public int Id { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryName { get; set; }
        public string IconUrl { get; set; }
    }
}
