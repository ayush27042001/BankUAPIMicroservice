using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public sealed class BiometricKycStatusApiResponse
    {
        public string statuscode { get; init; }
        public string status { get; init; }
        public string orderid { get; init; }
        public BiometricKycData data { get; init; }
    }

    public sealed class BiometricKycData
    {
        public string outletId { get; init; }
        public string outletAadhaarNumber { get; init; }
        public string action { get; init; }
        public string status { get; init; }
        public bool isFaceAuthAvailable { get; init; }
        public bool isBiometricKycMandatory { get; init; }
        public string pidOptionWadh { get; init; }
        public string referenceKey { get; init; }
        public string referenceKeyType { get; init; }
    }

    public sealed class BiometricKycApiResponse
    {
        public string statuscode { get; init; }
        public string status { get; init; }
    }

}
