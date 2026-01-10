using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Constants;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class AepsLoginService : IAepsLoginService
    {
        private readonly IUserRepository _userRepo;
        private readonly IInstantPayClient _instantPay;

        public AepsLoginService(
            IUserRepository userRepo,
            IInstantPayClient instantPay)
        {
            _userRepo = userRepo;
            _instantPay = instantPay;
        }

        public async ValueTask<LoginModel> ExecuteAsync(AepsLoginRequest req, CancellationToken ct)
        {
            if (!req.SpKey.Equals(AepsConstants.Login, StringComparison.OrdinalIgnoreCase))
            {
                return LoginModel.Fail("Invalid sp_key");
            }

            var outletId = await _userRepo
                .GetOutletIdAsync(Convert.ToInt32(req.UserId), ct)
                .ConfigureAwait(false);

            if (outletId is null)
            {
                return LoginModel.Fail("Outlet not found");
            }

            var payload = new AepsLoginApiPayload
            {
                externalRef = req.TransactionId,
                latitude = req.Latitude,
                longitude = req.Longitude,
                biometricData = new AepsLoginApiPayload.BiometricDataInsLogin
                {
                    encryptedAadhaar = req.PidData.EncryptedAadhaar,
                    dc = req.PidData.Dc,
                    ci = req.PidData.Ci,
                    hmac = req.PidData.Hmac,
                    dpId = req.PidData.DpId,
                    mc = req.PidData.Mc,
                    sessionKey = req.PidData.SessionKey,
                    mi = req.PidData.Mi,
                    rdsId = req.PidData.RdsId,
                    errCode = req.PidData.ErrCode,
                    errInfo = req.PidData.ErrInfo,
                    fCount = req.PidData.FCount,
                    pidData = req.PidData.PidData,
                    qScore = req.PidData.QScore,
                    nmPoints = req.PidData.NmPoints,
                    rdsVer = req.PidData.RdsVer
                }
            };

            var api = await _instantPay
                .AepsDailyLoginAsync(outletId, payload, ct)
                .ConfigureAwait(false);

            bool success =
                api.statuscode == AepsConstants.Txn ||
                api.statuscode == AepsConstants.Tup;

            var response = new aepsresponse
            {
                status = api.status,
                orderstatus = success ? "SUCCESS" : "FAILED",
                txntype = AepsConstants.LoginTxnType,
                agentid = "0",
                bankrefno = "0"
            };

            return new LoginModel
            {
                Status = success ? "SUCCESS" : "FAILED",
                Message = success
                    ? "Transaction Successful."
                    : "Transaction FAILED.",
                Data = new List<aepsresponse>(1) { response }
            };
        }
    }
}
