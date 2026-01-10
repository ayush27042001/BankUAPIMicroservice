using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
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
    }

}
