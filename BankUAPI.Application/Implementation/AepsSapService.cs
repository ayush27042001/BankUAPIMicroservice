using BankUAPI.Application.Interface;
using BankUAPI.SharedKernel.Constants;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation
{
    public sealed class AepsSapService : IAepsSapService
    {
        private readonly IInstantPayClient _api;
        private readonly ICommonRepositry _repo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AepsSapService(
            IInstantPayClient api,
            ICommonRepositry repo,
            IUserRepository userRepo,
            IConfiguration config)
        {
            _api = api;
            _repo = repo;
            _userRepo = userRepo;
            _config = config;
        }

        public async ValueTask<LoginModel> ExecuteAsync(AepsSapApiRequest req, CancellationToken ct)
        {
            string txnId = DateTime.UtcNow.ToString("yyMMddHHmmssfff");

            var outletId = await _userRepo
                .GetOutletIdAsync(Convert.ToInt32(req.UserId), ct);

            if (outletId is null)
                return LoginModel.Fail("Outlet not found");

            var apiReq = new AepsSapApiRequest
            {
                bankiin = req.bankiin,
                latitude = req.latitude,
                longitude = req.longitude,
                mobile = req.mobile,
                amount = req.amount,
                externalRef = txnId,
                biometricData = new AepsSapApiRequest.Biometric
                {
                    encryptedAadhaar = _repo.AESEncryption(req.Aadhar),
                    dc = req.biometricData.dc,
                    ci = req.biometricData.ci,
                    hmac = req.biometricData.hmac,
                    dpId = req.biometricData.dpId,
                    mc = req.biometricData.mc,
                    pidDataType = "X",
                    sessionKey = req.biometricData.sessionKey,
                    mi = req.biometricData.mi,
                    rdsId = req.biometricData.rdsId,
                    errCode = req.biometricData.errCode,
                    errInfo = req.biometricData.errInfo,
                    fCount = req.biometricData.fCount,
                    fType = "2",
                    iCount=0,
                    iType="",
                    pCount=0,
                    pType="",
                    srno = "N00115075",
                    sysid="",
                    ts="",
                    pidData = req.biometricData.pidData,
                    qScore = req.biometricData.qScore,
                    nmPoints = req.biometricData.nmPoints,
                    rdsVer = req.biometricData.rdsVer
                }
            };

            var apiResp = await _api.MiniStatementAsync(outletId, apiReq, ct);

            bool success = apiResp.statuscode is "TXN" or "TUP";

            await _repo.InsertAepsTxnAsync(
               Convert.ToInt32(req.UserId),
                txnId,
                req.mobile,
                0,
                success ? "SUCCESS" : "FAILED",
                AepsConstants.SAPProdKey,
                AepsConstants.SAPProdName,
                req.bankiin,
                req.Aadhar,
                apiResp,
                apiResp.ipay_uuid,
                apiResp.data?.operatorId,
                apiResp.orderid,
                ct);

            return new LoginModel
            {
                Status = success ? "SUCCESS" : "FAILED",
                Message = apiResp.status,
                Data = new List<aepsresponse>(1)
            {
                new()
                {
                    status = apiResp.status,
                    orderstatus = success ? "SUCCESS" : "FAILED",
                    txntype = "MINI STATEMENT",
                    acamount = "₹" + apiResp.data?.bankAccountBalance,
                    agentid = apiResp.orderid,
                    bankrefno = apiResp.data?.operatorId,
                    xmllist = apiResp.data?.miniStatement,
                    Commission = "0"
                }
            }
            };
        }

        
    }

}
