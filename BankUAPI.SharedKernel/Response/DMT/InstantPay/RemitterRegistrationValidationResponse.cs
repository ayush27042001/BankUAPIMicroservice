using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{
    public class RemitterRegistrationValidationResponse
    {
        public string StatusCode { get; set; } = default!;
        public string? ActCode { get; set; }
        public string Status { get; set; } = default!;
        public RemitterRegistrationValidationResponseData? Data { get; set; }
        public string? ipay_uuid { get; set; } = default!;
        public string? Timestamp { get; set; }
        public string? OrderId { get; set; }
        public string? Environment { get; set; }
        public string? InternalCode { get; set; }
        public bool? success { get; set; }
    }

    public class RemitterRegistrationValidationResponseData
    {
        public string? referenceID { get; set; }
    }
}
