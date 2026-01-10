using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed class AepsRequestDto
    {
        public string UserId { get; init; }
        public string SpKey { get; init; }
        public string MobileNo { get; init; }
        public string UserAgent { get; init; }
        public string AppVersion { get; init; }
        public AepsBiometricRequest Request { get; init; }
    }

    public sealed class AepsBiometricRequest
    {
        public decimal Amount { get; init; }
        public string AadhaarUid { get; init; }
        public string BankIin { get; init; }
        public string Latitude { get; init; }
        public string Longitude { get; init; }
        public string PidData { get; init; }
        public string SessionKey { get; init; }
        public string Hmac { get; init; }
        public string Ci { get; init; }
        public string Dc { get; init; }
    }


}
