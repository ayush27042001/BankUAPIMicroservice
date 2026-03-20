using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public class AadhaarRequest
    {
        public long UserId { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string AgreementId { get; set; }
        public string Otp { get; set; }
        public string RefId { get; set; }
    }
    public class AadhaarOtpResponse
    {
        public string RefId { get; set; }
        public string Status { get; set; }
    }
    public class AgreementSignResult
    {
        public string SignedFilePath { get; set; }
        public string FullName { get; set; }
    }
}
