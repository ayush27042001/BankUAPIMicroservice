using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.AddFund
{
    public class AddFundResponse
    {
        public string MobileNo { get; set; }
        public string Operator { get; set; }
        public string Amount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Status { get; set; }
        public string TxnID { get; set; }
        public string TxnDate { get; set; }

        public string QRImageBase64 { get; set; }
        public string PaymentUrl { get; set; }
        public string TxnRefId { get; set; }

        public string BHIM { get; set; }
        public string PhonePe { get; set; }
        public string Paytm { get; set; }
        public string GPay { get; set; }
    }
}
