using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class BeneficiaryVerifyRequest
    {
        public string? remitterMobileNumber { get; set; } = "";
        public string? referenceKey { get; set; } = "";
        public string? otp { get; set; } = "";
        public string? beneficiaryId { get; set; } = "";
        public string? UserId { get; set; } = "";
        public string? EndpointIp { get; set; } = "";
    }
}
