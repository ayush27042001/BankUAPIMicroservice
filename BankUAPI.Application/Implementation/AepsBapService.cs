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
    public sealed class AepsBapService : IAepsBapService
    {
        private readonly IInstantPayClient _api;
        private readonly ICommonRepositry _repo;
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AepsBapService(IInstantPayClient api, ICommonRepositry repo, IUserRepository userRepo, IConfiguration config)
        {
            _api = api;
            _repo = repo;
            _userRepo = userRepo;
            _config = config;
        }

        public async ValueTask<LoginModel> ExecuteAsync(AepsBapRequest req, CancellationToken ct)
        {
            string txnId = DateTime.Now.ToString("yyMMddHHmmssfff");

            var outletId = await _userRepo
                .GetOutletIdAsync(Convert.ToInt32(req.UserId), ct)
                .ConfigureAwait(false);

            if (outletId is null)
            {
                return LoginModel.Fail("Outlet not found");
            }

            var apiReq = new AepsBapApiRequest
            {
                bankiin = req.BankIin,
                latitude = req.Latitude,
                longitude = req.Longitude,
                mobile = req.Mobile,
                externalRef = txnId,
                biometricData = new AepsBapApiRequest.Biometric
                {
                    encryptedAadhaar = _repo.AESEncryption(req.Aadhaar),
                    dc = req.PidData.Dc,
                    ci = req.PidData.Ci,
                    hmac = req.PidData.Hmac,
                    dpId = req.PidData.DpId,
                    mc = req.PidData.Mc,
                    pidDataType = "X",
                    sessionKey = req.PidData.SessionKey,
                    mi = req.PidData.Mi,
                    rdsId = req.PidData.RdsId,
                    errCode = req.PidData.ErrCode,
                    errInfo = req.PidData.ErrInfo,
                    fCount = req.PidData.FCount,
                    fType = "2",
                    iCount =0,
                    iType= "",
                    pCount=0,
                    pType="",
                    srno= "N00115075",
                    sysid="",
                    ts="",
                    pidData = req.PidData.PidData,
                    qScore = req.PidData.QScore,
                    nmPoints = req.PidData.NmPoints,
                    rdsVer = req.PidData.RdsVer
                }
            };

            var apiResp = await _api
                .BalanceEnquiryAsync(outletId, apiReq, ct);

            bool success =
                apiResp.statuscode is "TXN" or "TUP";

            await _repo.InsertAepsTxnAsync(
                req.UserId,
                txnId,
                req.Mobile,
                0,
                success ? "SUCCESS" : "FAILED",
                AepsConstants.BAPProdKey,
                AepsConstants.BAPProdName,
                req.BankIin,
                req.Aadhaar,
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
                    txntype = "BALANCE ENQUIRY",
                    acamount = "₹" + apiResp.data?.bankAccountBalance,
                    agentid = apiResp.orderid,
                    bankrefno = apiResp.data?.operatorId,
                    Commission = "0"
                }
            }
            };
        }

        
    }

}
