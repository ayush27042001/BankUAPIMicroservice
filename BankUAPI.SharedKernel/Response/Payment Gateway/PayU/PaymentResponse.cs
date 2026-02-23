using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Payment_Gateway.PayU
{
    public class PaymentResponse
    {
        public string TxnId { get; set; }
        public string PayUMoneyId { get; set; }
        public string Status { get; set; }
        public string BankRef { get; set; }
        public string Message { get; set; }
    }

}
