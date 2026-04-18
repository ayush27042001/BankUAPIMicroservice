using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.ZohoMailSent
{
    public class EmailRequest
    {
        public string? Email { get; set; }
        public string? DeviceId { get; set; }
    }

    public class VerifyEmailOtpRequest
    {
        public string? Email { get; set; }
        public string? Otp { get; set; }
        public string? DeviceId { get; set; }
    }

    public class RiskResult
    {
        public int? Score { get; set; }
        public string? Decision { get; set; }
    }
}
