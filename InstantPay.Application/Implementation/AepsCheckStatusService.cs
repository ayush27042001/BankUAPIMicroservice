using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class AepsCheckStatusService : IAepsCheckStatusService
    {
        private readonly IUserRepository _userRepo;
        private readonly IInstantPayClient _instantPay;
        public AepsCheckStatusService(
        IUserRepository userRepo, IInstantPayClient instantPay)
        {
            _userRepo = userRepo;
            _instantPay = instantPay;
        }

        public async Task<LoginModel> ExecuteAsync(
        AepsCheckStatusRequestDto request,
        CancellationToken ct)
        {
            if (request.SpKey != "checkstatus")
                return LoginModel.Fail("Invalid sp_key");

            var user = await _userRepo.GetOutletAsync(request.UserId, ct);
            if (user == null)
                return LoginModel.Fail("Unauthorized user");

            var jsonResponse = await _instantPay.CheckStatusAsync(user.OutletId, ct);

            if (string.IsNullOrWhiteSpace(jsonResponse))
                return LoginModel.Fail("InstantPay no response");

            var json = JsonDocument.Parse(jsonResponse).RootElement;

            string statusCode = json.GetProperty("statuscode").GetString();
            string actCode = json.TryGetProperty("actcode", out var a) ? a.GetString() : "";
            string status = json.TryGetProperty("status", out var s) ? s.GetString() : "";
            string ipayUuid = json.TryGetProperty("ipay_uuid", out var u) ? u.GetString() : "";

            var data = new List<aepsresponse>();

            if (statusCode == "RPI")
            {
                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "ERROR",
                    txntype = "LOGIN",
                    bankrefno = ipayUuid
                });

                return new LoginModel
                {
                    Status = "ERROR",
                    Message = status,
                    Data = data
                };
            }

            if (statusCode == "TXN" && actCode == "LOGINREQUIRED")
            {
                data.Add(new aepsresponse
                {
                    status = "Outlet login required",
                    orderstatus = "LOGIN_REQUIRED",
                    txntype = "LOGIN",
                    bankrefno = ipayUuid
                });

                return new LoginModel
                {
                    Status = "LOGIN_REQUIRED",
                    Message = "Outlet login required",
                    Data = data
                };
            }

            if (statusCode == "TXN")
            {
                data.Add(new aepsresponse
                {
                    status = status,
                    orderstatus = "SUCCESS",
                    txntype = "LOGIN",
                    bankrefno = ipayUuid
                });

                return new LoginModel
                {
                    Status = "SUCCESS",
                    Message = status,
                    Data = data
                };
            }

            data.Add(new aepsresponse
            {
                status = status,
                orderstatus = "FAILED",
                txntype = "LOGIN",
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
