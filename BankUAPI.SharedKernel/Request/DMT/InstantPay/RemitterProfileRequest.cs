using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class RemitterProfileRequest
    {
        public string UserId { get; set; } = "";
        public string MobileNumber { get; set; } = default!;
        public string TxnMode { get; set; } = "ALL";
        public string IftEnable { get; set; } = "YES";
        public string Source { get; set; } = "WEB"; // "WEB" or "APP"
        public string EndpointIp { get; set; } = default!;
        public string provider { get; set; } = default!;
    }
}
