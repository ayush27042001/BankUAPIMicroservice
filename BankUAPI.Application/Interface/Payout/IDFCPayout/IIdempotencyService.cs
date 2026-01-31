using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payout.IDFCPayout
{
    public interface IIdempotencyService
    {
        Task<string?> GetExistingResponseAsync(string key, string requestHash, string clientCode);
        Task SaveAsync(string key, string requestHash, string responseJson, string clientCode);
    }
}
