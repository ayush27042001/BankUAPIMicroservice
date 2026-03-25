using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.BankAccount
{
    public class PanVerifyResult
    {
        public bool? IsValid { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }

    public class AadhaarOtpResult
    {
        public bool? Success { get; set; }
        public string? RefId { get; set; }
        public string? Message { get; set; }
    }

    public class AadhaarVerifyResult
    {
        public bool? Success { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
    }
    public class GstVerifyResult
    {
        public bool? IsValid { get; set; }
        public string? LegalName { get; set; }
        public string? TradeName { get; set; }
    }

    public class CinVerifyResult
    {
        public bool? IsValid { get; set; }
        public string? CompanyName { get; set; }
        public bool? IsDirectorMatched { get; set; }
    }
}
