using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface ICommonRepositry
    {
        Task InsertAepsTxnAsync(int userId, string transactionId, string mobile, decimal amount, string status, string prodKey, string prodName, string bankIin, string aadhaar, object apiResponse, string ipayUuid, string operatorId, string orderId, CancellationToken ct);
        string AESEncryption(string data);
        Task InsertInsPayBankDetails(INSPayBankDetail request, CancellationToken ct);
        Task<List<INSPayBankDetail>> FetchBankDetails();
        Task AddDmtTxnReportAsync(Registration userData, string serviceName, string operatorId, string operatorName, string mobile, string accountNo, decimal txnAmount, decimal retailerCharge, string status, string apiName, object apiResponse, string transactionId, string? orderId, string? BankName, string? BeneName, string? Ifsccode, decimal? OldBalance, CancellationToken ct);
        Task<WalletCheckResonse> WalletCheckValidationAsync(int userId, decimal txnAmount, CancellationToken ct = default);
        Task<WalletCheckResonse> RefundWalletBalance(int userId, decimal amount, string Remarks, CancellationToken ct = default);
        Task<Dictionary<int, decimal>> GetLatestBalancesAsync(params int?[] userIds);
    }

}
