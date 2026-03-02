using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.AddFund
{
    public interface IAddFundClient
    {
        Task<string> CreateOrderAsync(object payload, CancellationToken ct);
        Task<string> CheckStatusAsync(object payload, CancellationToken ct);
    }
}
