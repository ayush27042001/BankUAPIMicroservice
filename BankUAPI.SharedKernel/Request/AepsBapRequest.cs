using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed record AepsBapRequest
    (
        int UserId,
        string BankIin,
        string Mobile,
        string Aadhaar,
        string Latitude,
        string Longitude,
        BiometricPidData PidData
    );

    public sealed class AepsBapApiRequest
    {
        public string bankiin { get; init; }
        public string latitude { get; init; }
        public string longitude { get; init; }
        public string mobile { get; init; }
        public string externalRef { get; init; }
        public Biometric biometricData { get; init; }

        public sealed class Biometric
        {
            public string encryptedAadhaar { get; init; }
            public string dc { get; init; }
            public string ci { get; init; }
            public string hmac { get; init; }
            public string dpId { get; init; }
            public string mc { get; init; }
            public string pidDataType { get; init; } = "X";
            public string sessionKey { get; init; }
            public string mi { get; init; }
            public string rdsId { get; init; }
            public string errCode { get; init; }
            public string errInfo { get; init; }
            public string fCount { get; init; }
            public string fType { get; init; } = "2";
            public int iCount { get; init; } = 0;
            public string iType { get; init; } = "";
            public int pCount { get; init; } = 0;
            public string pType { get; init; } = "";
            public string srno { get; init; } = "N00115075";
            public string sysid { get; init; } = "";
            public string ts { get; init; } = "";
            public string pidData { get; init; }
            public string qScore { get; init; }
            public string nmPoints { get; init; }
            public string rdsVer { get; init; }
        }
    }

}
