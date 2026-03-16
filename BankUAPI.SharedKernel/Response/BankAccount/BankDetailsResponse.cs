using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.BankAccount
{
    public class BankDetailsResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<BankDetail> Banks { get; set; }
    }

    public class BankDetail
    {
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public string BankName { get; set; }
        public string AccountHolder { get; set; }
        public string Type { get; set; } 
    }
}
