using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed record AepsBiometricKycRequest(
    string UserId,
    string SpKey,
    string ReferenceKey,
    string Latitude,
    string Longitude,
    string TransactionId,
    BiometricPidData PidData
    );

    public sealed record BiometricPidData
    (
    string EncryptedAadhaar,
    string Dc,
    string Ci,
    string Hmac,
    string DpId,
    string Mc,
    string SessionKey,
    string Mi,
    string RdsId,
    string ErrCode,
    string ErrInfo,
    string FCount,
    string ICount,
    string PCount,
    string PType,
    string Srno,
    string PidData,
    string QScore,
    string NmPoints,
    string RdsVer
    );


    public sealed class BiometricKycApiPayload
    {
        public string referenceKey { get; init; }
        public string latitude { get; init; }
        public string longitude { get; init; }
        public string externalRef { get; init; }
        public BiometricData biometricData { get; init; }

        public sealed class BiometricData
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
            public string iCount { get; init; }
            public string iType { get; init; } = "0";
            public string pCount { get; init; }
            public string pType { get; init; }
            public string srno { get; init; }
            public string sysid { get; init; } = "";
            public string ts { get; init; } = "";
            public string pidData { get; init; }
            public string qScore { get; init; }
            public string nmPoints { get; init; }
            public string rdsVer { get; init; }
        }
    }



}
