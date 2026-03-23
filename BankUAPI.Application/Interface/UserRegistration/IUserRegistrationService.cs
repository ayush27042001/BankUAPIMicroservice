using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.UserRegistration
{
    public interface IUserRegistrationService
    {
        Task<ApiResponse<Step1Response>> Step1Async(Step1Request request);
        Task<ApiResponse<MessageResponse>> Step2Async(Step2Request request);
        Task<ApiResponse<MessageResponse>> Step3Async(Step3Request request);
        Task<ApiResponse<MessageResponse>> Step4Async(Step4Request request);
        Task<ApiResponse<MessageResponse>> CompleteAsync(Step5Request request);
        Task<ApiResponse<AadhaarOtpResult>> SendAadhaarOtpAsync(AadhaarOtpRequest req);
    }
}
