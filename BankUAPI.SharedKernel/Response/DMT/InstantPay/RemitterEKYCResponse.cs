using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{

    public class RemitterEKYCResponse
    {
        public string? StatusCode { get; set; }
        public string? ActCode { get; set; }
        public string? Status { get; set; }
        public BeneficiaryData? Data { get; set; }
        public string? Timestamp { get; set; }
        public string? Ipay_Uuid { get; set; }
        public string? OrderId { get; set; }
        public string? Environment { get; set; }
        public bool Success => StatusCode == "TXN";
    }

    public class BeneficiaryData
    {
        public string? PoolReferenceId { get; set; }
        public PoolInfo? Pool { get; set; }
    }

    public class PoolInfo
    {
        public string? Account { get; set; }
        public string? OpeningBal { get; set; }
        public string? Mode { get; set; }
        public string? Amount { get; set; }
        public string? ClosingBal { get; set; }
    }

}
