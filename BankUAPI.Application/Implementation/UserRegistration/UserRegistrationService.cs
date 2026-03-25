using BankUAPI.Application.Interface.UserRegistration;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Request.BankAccount;
using BankUAPI.SharedKernel.Response.BankAccount;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.UserRegistration
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly AppDbContext _db;
        private readonly ICashfreeService _cashfree;

        public UserRegistrationService(AppDbContext db, ICashfreeService cashfree)
        {
            _db = db;
            _cashfree = cashfree;
        }

        // ================= STEP 1 =================
        public async Task<ApiResponse<Step1Response>> Step1(Step1Request req)
        {
            if (string.IsNullOrEmpty(req.Pan) || string.IsNullOrEmpty(req.Mpin))
                return Error<Step1Response>("All fields required");

            if (req.Mpin != req.ConfirmMpin)
                return Error<Step1Response>("MPIN mismatch");

            if (!Regex.IsMatch(req.Pan, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                return Error<Step1Response>("Invalid PAN");
            bool exists = await _db.Registrations
          .AnyAsync(x => x.MobileNo == req.Mobile || x.Panno == req.Pan);

            if (exists)
                return Error<Step1Response>("User already exists");
            var panResult = await _cashfree.VerifyPan(req.Pan);

            if (!(panResult.IsValid ?? false) || !string.Equals(panResult.Type, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))

            {
                return Error<Step1Response>("PAN verification failed. Enter valid individual PAN");
            }

            string name = panResult.Name?.ToUpper();
            var user = new Infrastructure.Sql.Entities.Registration
            {
                FullName = name,
                MobileNo = req.Mobile,
                Mpin = req.Mpin,
                Panno = req.Pan,
                RegistrationStatus = "Pan",
                UserType = req.UserType,
                RegDate = Convert.ToString(DateTime.Now),
                Status = "1"
            };

            _db.Registrations.Add(user);
            await _db.SaveChangesAsync();

            return Success(new Step1Response
            {
                UserId = user.RegistrationId,
                Name = name
            });
        }

        // ================= STEP 2 =================
        public async Task<ApiResponse<MessageResponse>> Step2(Step2Request req)
        {
            var user = await _db.Registrations.FindAsync(req.RegistrationId);

            if (user == null)
                return Error<MessageResponse>("User not found");

            user.AccountType = req.AccountType;
            user.BusinessType = req.BusinessType;
            user.Email = req.Email;
            user.RegistrationStatus = "Bussiness";

            await _db.SaveChangesAsync();

            return Success(new MessageResponse { Result = "Step2 completed" });
        }

        // ================= STEP 3 =================
        public async Task<ApiResponse<MessageResponse>> Step3(Step3Request req)
        {
            var user = await _db.Registrations.FindAsync(req.RegistrationId);

            if (user == null)
                return Error<MessageResponse>("User not found");

            var panResult = await _cashfree.VerifyBusinessPan(req.BusinessPan);

            if (panResult == null || !(panResult.IsValid ?? false))
                return Error<MessageResponse>("Invalid company PAN");
            string panName = panResult.Name?.ToUpper();

            string proofType = req.BusinessProof?.Trim().ToUpper();
            user.CompanyName = req.BusinessName;
            if (proofType == "GST")
            {
                var gstResult = await _cashfree.VerifyGst(req.ProofNumber, req.BusinessName);

                if (gstResult == null)
                    return Error<MessageResponse>("GST verification failed");

                if (!(gstResult.IsValid ?? false))
                    return Error<MessageResponse>("Invalid GST number");

                string gstName = gstResult.LegalName ?? gstResult.TradeName;
                user.CompanyName = gstName;
            }

            else if (proofType == "CIN")
            {
                var cinResult = await _cashfree.VerifyCin(
                    req.ProofNumber,
                    panName
                );

                if (cinResult == null)
                    return Error<MessageResponse>("CIN verification failed");

                if (!(cinResult.IsValid ?? false))
                    return Error<MessageResponse>("Invalid CIN");

                if (!(cinResult.IsDirectorMatched ?? false))
                    return Error<MessageResponse>("Director name does not match PAN");

                user.CompanyName = cinResult.CompanyName;
            }

            user.BusinessProof = req.BusinessProof;
            user.BusinessProofNo = req.ProofNumber;
            user.NatureOfBusiness = req.NatureOfBusiness;
            user.BusinessPan = req.BusinessPan;
            user.RegistrationStatus = "Bussiness";

            await _db.SaveChangesAsync();

            return Success(new MessageResponse { Result = "Step3 completed" });
        }

        // ================= STEP 4 =================
        public async Task<ApiResponse<MessageResponse>> Step4(Step4Request req)
        {
            var user = await _db.Registrations.FindAsync(req.RegistrationId);

            if (user == null)
                return Error<MessageResponse>("User not found");

            if (string.IsNullOrWhiteSpace(req.Otp) || string.IsNullOrWhiteSpace(req.RefId))
            {
                return Error<MessageResponse>("OTP and RefId required");
            }

                var result = await _cashfree.VerifyAadhaar(
                    req.Otp,
                    req.RefId,
                    user.FullName
                );

                if (!(result.Success ?? false))
                    return Error<MessageResponse>("Aadhaar verification failed or name mismatch");

                user.AadharNo = req.Aadhaar;
                user.AddressUser = result.Address;
                user.Gender = result.Gender;
                user.Dob = result.DOB;
                user.State = result.State;
                user.Postal = result.Pincode;
                user.RegistrationStatus = "Aadhar";

                await _db.SaveChangesAsync();

                return Success(new MessageResponse { Result = "Aadhaar verified" });
        }

        // ================= STEP 5 =================
        public async Task<ApiResponse<MessageResponse>> Complete(Step5Request req)
        {
            var user = await _db.Registrations.FindAsync(req.RegistrationId);

            if (user == null)
                return Error<MessageResponse>("User not found");

            user.Latitude = req.Latitude;
            user.Longitude = req.Longitude;
            user.Ipaddress = req.IpAddress;
            user.RmId = req.RmId;
            user.FaceVerificationResult = "true";
            user.CommissionPlanId = req.PlanId;
            user.RegistrationStatus = "Done";

            await _db.SaveChangesAsync();

            return Success(new MessageResponse { Result = "Registration completed" });
        }
        public async Task<ApiResponse<AadhaarOtpResult>> SendAadhaarOtp(AadhaarOtpRequest req)
        {
            var user = await _db.Registrations.FindAsync(req.RegistrationId);

            if (user == null)
                return Error<AadhaarOtpResult>("User not found");

            if (string.IsNullOrWhiteSpace(req.Aadhaar))
                return Error<AadhaarOtpResult>("Aadhaar is required");

            var result = await _cashfree.SendAadhaarOtp(req.Aadhaar);

            if (!(result.Success ?? false))
                return Error<AadhaarOtpResult>(result.Message ?? "OTP failed");

            return Success(result);
        }
        private ApiResponse<T> Success<T>(T data)
        {
            return new ApiResponse<T>
            {
                Status = "SUCCESS",
                Message = "OK",
                Data = data
            };
        }

        private ApiResponse<T> Error<T>(string msg)
        {
            return new ApiResponse<T>
            {
                Status = "ERR",
                Message = msg,
                Data = default
            };
        }
    }
}
