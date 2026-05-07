using BankUAPI.SharedKernel.Request.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.UserRegistration
{
    public interface ICashfreeService
    {
        Task<PanVerifyResult> VerifyPan(string pan);
        Task<PanVerifyResult> VerifyBusinessPan(string pan);
        Task<AadhaarOtpResult> SendAadhaarOtp(string aadhaar);
        Task<AadhaarVerifyResult> VerifyAadhaar(string otp, string refId, string panName);
        Task<GstVerifyResult> VerifyGst(string gst, string businessName);
        Task<CinVerifyResult> VerifyCin( string cin, string panName);
        Task LogApiCall(string userId, string request, string response, string apiType);
    }
}
