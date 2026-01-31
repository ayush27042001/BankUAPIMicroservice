using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class GenerateTransactionOTPRequest
    {
        public string remitterMobileNumber { get; set; } = "";
        public string amount { get; set; } = "";
        public string referenceKey { get; set; } = "";
        public string ip { get; set; } = "";
        public string userId { get; set; } = "";
    }
}
