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
        Task<PanVerifyResult> VerifyPan(string pan, string RegId);
        Task<PanVerifyResult> VerifyBusinessPan(string pan, string RegId);
        Task<AadhaarOtpResult> SendAadhaarOtp(string aadhaar, string RegId);
        Task<AadhaarVerifyResult> VerifyAadhaar(string otp, string refId, string panName, string RegId);
        Task<GstVerifyResult> VerifyGst(string gst, string businessName, string RegId);
        Task<CinVerifyResult> VerifyCin( string cin, string panName, string RegId);
        Task LogApiCall(string userId, string request, string response, string apiType);
    }
}
