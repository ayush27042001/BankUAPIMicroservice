using BankUAPI.SharedKernel.Request.BillPayement;
using BankUAPI.SharedKernel.Response.BillPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IBillPaymentService
    {
        Task<BillPaymentResponse> GetBillers(BillPaymentRequest request, CancellationToken ct);
        Task<OperatorResponse> GetOperators(OperatorRequest request, CancellationToken ct);
    }
}
