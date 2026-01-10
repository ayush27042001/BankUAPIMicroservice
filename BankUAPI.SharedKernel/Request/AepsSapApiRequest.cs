using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed class AepsSapApiRequest
    {
        public string UserId { get; set; }
        public string bankiin { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string mobile { get; set; }
        public string amount { get; set; }
        public string externalRef { get; set; }
        public string Aadhar { get; set; }
        public Biometric biometricData { get; set; }

        public sealed class Biometric
        {
            public string encryptedAadhaar { get; set; }
            public string dc { get; set; }
            public string ci { get; set; }
            public string hmac { get; set; }
            public string dpId { get; set; }
            public string mc { get; set; }
            public string pidDataType { get; set; }
            public string sessionKey { get; set; }
            public string mi { get; set; }
            public string rdsId { get; set; }
            public string errCode { get; set; }
            public string errInfo { get; set; }
            public string fCount { get; set; }
            public string fType { get; set; }
            public int iCount { get; set; }
            public string iType { get; set; }
            public int pCount { get; set; }
            public string pType { get; set; }
            public string srno { get; set; }
            public string sysid { get; set; }
            public string ts { get; set; }
            public string pidData { get; set; }
            public string qScore { get; set; }
            public string nmPoints { get; set; }
            public string rdsVer { get; set; }
        }
    }

}
