using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Constants;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class AepsBiometricKycService: IAepsBiometricKycService
    {
        private readonly IUserRepository _userRepo;
        private readonly IInstantPayClient _instantPay;

        public AepsBiometricKycService(IUserRepository userRepo, IInstantPayClient instantPay)
        {
            _userRepo = userRepo;
            _instantPay = instantPay;
        }

        public async ValueTask<LoginModel> CheckEKYCStatus(AepsBiometricKycStatusRequest req, CancellationToken ct)
        {
            if (!string.Equals(
                req.SpKey,
                AepsConstants.BiometricKycStatus,
                StringComparison.OrdinalIgnoreCase))
            {
                return LoginModel.Fail("Invalid sp_key");
            }

            var outletId = await _userRepo.GetOutletIdAsync(Convert.ToInt32(req.UserId), ct).ConfigureAwait(false);

            if (outletId is null)
            {
                return LoginModel.Fail("Outlet not found");
            }
           
            var api = await _instantPay
                .GetBiometricKycStatusAsync(Convert.ToString(outletId), ct);

            bool success = api.statuscode == AepsConstants.Txn || api.statuscode == AepsConstants.Tup;

            var response = new aepsresponse
            {
                status = api.status,
                orderstatus = success ? "SUCCESS" : "FAILED",
                txntype = AepsConstants.TxType,
                orderamount = "0",
                acamount = "0",
                agentid = outletId,
                bankrefno = api.orderid,
                xmllist = success ? api.data : null,
                OldBalance = "0",
                NewBalance = "0",
                Commission = "0"
            };

            return new LoginModel
            {
                Status = success ? "SUCCESS" : "FAILED",
                Message = api.status,
                Data = new List<aepsresponse>(1) { response }
            };
        }

        public async ValueTask<LoginModel> ExecuteAsync(AepsBiometricKycRequest req, CancellationToken ct)
        {
            if (!req.SpKey.Equals(
                AepsConstants.BiometricKyc,
                StringComparison.OrdinalIgnoreCase))
                return LoginModel.Fail("Invalid sp_key");

            var outletId = await _userRepo
                .GetOutletIdAsync(Convert.ToInt32(req.UserId), ct)
                .ConfigureAwait(false);

            if (outletId is null)
                return LoginModel.Fail("Outlet not found");

            var payload = new BiometricKycApiPayload
            {
                referenceKey = req.ReferenceKey,
                latitude = req.Latitude,
                longitude = req.Longitude,
                externalRef = req.TransactionId,
                biometricData = new BiometricKycApiPayload.BiometricData
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
                    iCount = req.PidData.ICount,
                    pCount = req.PidData.PCount,
                    pType = req.PidData.PType,
                    srno = req.PidData.Srno,
                    pidData = req.PidData.PidData,
                    qScore = req.PidData.QScore,
                    nmPoints = req.PidData.NmPoints,
                    rdsVer = req.PidData.RdsVer
                }
            };

            var api = await _instantPay
                .SubmitBiometricKycAsync(outletId, payload, ct)
                .ConfigureAwait(false);

            bool success =
                api.statuscode == AepsConstants.Txn ||
                api.statuscode == AepsConstants.Tup;

            var response = new aepsresponse
            {
                status = api.status,
                orderstatus = success ? "SUCCESS" : "FAILED",
                txntype = AepsConstants.BioMetricTxnType,
                agentid = outletId,
                bankrefno = req.TransactionId
            };

            return new LoginModel
            {
                Status = success ? "SUCCESS" : "FAILED",
                Message = success
                    ? "Biometric KYC completed successfully."
                    : api.status ?? "Biometric KYC failed.",
                Data = new List<aepsresponse>(1) { response }
            };
        }
    }
}
