using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed class AepsSignupRequestDto
    {
        public string UserId { get; init; }
        public string SpKey { get; init; } // "signup"

        public string Mobile { get; init; }
        public string Pan { get; init; }
        public string Aadhaar { get; init; }

        public string FirmName { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Pincode { get; init; }

        public string Latitude { get; init; }
        public string Longitude { get; init; }
    }

    public sealed class AepsSignupValidateRequestDto
    {
        public string UserId { get; init; }
        public string SpKey { get; init; } 

        public string Mobile { get; init; }
        public string Otp { get; init; }
        public string ReferenceId { get; init; } // ipay_uuid from signup
    }


}
