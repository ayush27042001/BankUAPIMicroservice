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
        Task<PanVerifyResult> VerifyPanAsync(string pan);
        Task<PanVerifyResult> VerifyBusinessPanAsync(string pan);
        Task<AadhaarOtpResult> SendAadhaarOtpAsync(string aadhaar);
        Task<AadhaarVerifyResult> VerifyAadhaarAsync(string otp, string refId, string panName);
        Task<GstVerifyResult> VerifyGstAsync(string gst, string businessName);
        Task<CinVerifyResult> VerifyCinAsync( string cin, string panName);
    }
}
