using BankUAPI.SharedKernel.ENUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.ZohoMailService
{
    public interface IEmailOtpService
    {
        Task<bool> CanSendOtp(string email);
        Task<string> GenerateAndSaveOtp(string email, string deviceId);
        Task<OtpVerifyStatus> VerifyOtp(string email, string otp, string deviceId);
    }
}
