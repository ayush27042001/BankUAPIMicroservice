using BankUAPI.SharedKernel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> GetInvoicesAsync(string userId, CancellationToken cn);
    }
}
