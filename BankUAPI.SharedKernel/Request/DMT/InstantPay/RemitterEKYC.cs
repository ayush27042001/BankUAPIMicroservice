using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankUAPI.SharedKernel.Request.BiometricKycApiPayload;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class RemitterEKYC
    {
        public string MobileNumber { get; set; } = default!;
        public string ReferenceKey { get; set; } = default!;
        public string Latitude { get; set; } = default!;
        public string Longitude { get; set; } = default!;
        public string ExternalRef { get; set; } = default!;
        public string ConsentTaken { get; set; } = "Y";
        public string? CaptureType { get; set; } // FINGER / FACE
        public BiometricDataRemitterEKYC? BiometricData { get; set; }
        public string EndpointIp { get; set; } = default!;
        public string? UserId { get; set; }
    }

    public class BiometricDataRemitterEKYC
    {
        public string ci { get; set; }
        public string hmac { get; set; }
        public string pidData { get; set; }
        public string ts { get; set; }
        public string dc { get; set; }
        public string mi { get; set; }
        public string dpId { get; set; }
        public string mc { get; set; }
        public string rdsId { get; set; }
        public string rdsVer { get; set; }
        public string Skey { get; set; }
        public string srno { get; set; }
    }


}
