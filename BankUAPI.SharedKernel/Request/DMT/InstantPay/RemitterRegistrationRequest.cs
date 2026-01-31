using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class RemitterRegistrationRequest
    {
        public string UserId { get; set; } = "";
        public string referenceKey { get; set; } = default!;
        public string TxnMode { get; set; } = "";
        public string Aadhar { get; set; } = "";
        public string mobileNumber { get; set; } = "";
        public string otp { get; set; } = "";
        public string Source { get; set; } = "WEB"; 
        public string provider { get; set; } = default!;
        public string EndpointIp { get; set; } = "0.0";
    }
}
