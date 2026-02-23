using BankUAPI.SharedKernel.Request.Payment_Gateway.PayU;
using BankUAPI.SharedKernel.Response.Payment_Gateway.PayU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Payment_Gateway.PayU
{
    public interface IPayUPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(string txnId);
    }
}
