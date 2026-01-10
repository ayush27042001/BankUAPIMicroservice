using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class AepsSignupService : IAepsSignupService
    {
        private readonly IUserRepository _userRepo;
        private readonly IInstantPayClient _instantPay;

        public AepsSignupService(
        IUserRepository userRepo,
        IInstantPayClient instantPay)
        {
            _userRepo = userRepo;
            _instantPay = instantPay;
        }

        public async Task<LoginModel> ExecuteAsync(AepsSignupRequestDto request, CancellationToken ct)
        {
            if (request.SpKey != "signup")
                return LoginModel.Fail("Invalid sp_key");

            var profile = await _userRepo.GetSignupProfileAsync(request.UserId, ct);
            if (profile == null)
                return LoginModel.Fail("Unauthorized user");

            var payload = new
            {
                mobile = request.Mobile,
                pan = request.Pan,
                aadhaar = request.Aadhaar,
                firm = request.FirmName,
                address = request.Address,
                city = request.City,
                state = request.State,
                pincode = request.Pincode,
                latitude = request.Latitude,
                longitude = request.Longitude,
                name = profile.Name,
                email = profile.Email
            };

            var jsonResponse = await _instantPay.SignupAsync(payload, ct);
            if (string.IsNullOrWhiteSpace(jsonResponse))
                return LoginModel.Fail("InstantPay not responding");

            var json = JsonDocument.Parse(jsonResponse).RootElement;

            string statusCode = json.GetProperty("statuscode").GetString();
            string status = json.GetProperty("status").GetString();
            string outletId = json.TryGetProperty("outletid", out var o) ? o.GetString() : "";
            string ipayUuid = json.TryGetProperty("ipay_uuid", out var u) ? u.GetString() : "";

            var data = new List<aepsresponse>();

            // OTP Required
            if (statusCode == "TXN" && status.Contains("OTP", StringComparison.OrdinalIgnoreCase))
            {
                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "OTP_REQUIRED",
                    txntype = "SIGNUP",
                    bankrefno = ipayUuid
                });

                return new LoginModel
                {
                    Status = "OTP_REQUIRED",
                    Message = status,
                    Data = data
                };
            }

            // SUCCESS
            if (statusCode == "TXN" && !string.IsNullOrEmpty(outletId))
            {
                await _userRepo.SaveOutletAsync(request.UserId, outletId, ct);

                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "SUCCESS",
                    txntype = "SIGNUP",
                    bankrefno = outletId
                });

                return new LoginModel
                {
                    Status = "SUCCESS",
                    Message = "Signup successful",
                    Data = data
                };
            }

            // FAILED
            data.Add(new aepsresponse
            {
                status = status,
                orderstatus = "FAILED",
                txntype = "SIGNUP",
                bankrefno = ipayUuid
            });

            return new LoginModel
            {
                Status = "FAILED",
                Message = status,
                Data = data
            };
        }

        public async Task<LoginModel> SignUpValidate(AepsSignupValidateRequestDto request, CancellationToken ct)
        {
            if (request.SpKey != "signupvalidate")
            {
                return LoginModel.Fail("Invalid sp_key");
            }

            var user = await _userRepo.GetSignupProfileAsync(request.UserId, ct);
            if (user == null)
            {
                return LoginModel.Fail("Unauthorized user");
            }

            var payload = new
            {
                mobile = request.Mobile,
                otp = request.Otp,
                referenceid = request.ReferenceId
            };

            var jsonResponse = await _instantPay.SignupValidateAsync(payload, ct);
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return LoginModel.Fail("InstantPay not responding");
            }

            var json = JsonDocument.Parse(jsonResponse).RootElement;

            string statusCode = json.GetProperty("statuscode").GetString();
            string status = json.GetProperty("status").GetString();
            string outletId = json.TryGetProperty("outletid", out var o) ? o.GetString() : "";
            string ipayUuid = json.TryGetProperty("ipay_uuid", out var u) ? u.GetString() : "";
            var data = new List<aepsresponse>();
            if (statusCode == "TXN" && !string.IsNullOrEmpty(outletId))
            {
                await _userRepo.SaveOutletAsync(request.UserId, outletId, ct);

                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "SUCCESS",
                    txntype = "SIGNUP_VALIDATE",
                    bankrefno = outletId
                });

                return new LoginModel
                {
                    Status = "SUCCESS",
                    Message = "Outlet activated successfully",
                    Data = data
                };
            }
            if (statusCode == "ERR" &&
                status.Contains("OTP", StringComparison.OrdinalIgnoreCase))
            {
                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "INVALID_OTP",
                    txntype = "SIGNUP_VALIDATE",
                    bankrefno = ipayUuid
                });

                return new LoginModel
                {
                    Status = "INVALID_OTP",
                    Message = status,
                    Data = data
                };
            }

            data.Add(new aepsresponse
            {
                status = status,
                orderstatus = "FAILED",
                txntype = "SIGNUP_VALIDATE",
                bankrefno = ipayUuid
            });

            return new LoginModel
            {
                Status = "FAILED",
                Message = status,
                Data = data
            };
        }
    }
}
