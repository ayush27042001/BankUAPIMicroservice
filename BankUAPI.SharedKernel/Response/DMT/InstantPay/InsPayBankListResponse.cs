using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{
    public class InsPayBankListResponse
    {
        public string? StatusCode { get; set; }
        public string? ActCode { get; set; }
        public string? Status { get; set; }
        public List<BankInfo>? Data { get; set; }
        public string? Timestamp { get; set; }
        public string? Ipay_Uuid { get; set; }
        public string? OrderId { get; set; }
        public string? Environment { get; set; }

        public bool? Success => StatusCode == "TXN";
    }

    public class BankInfo
    {
        public int? BankId { get; set; }
        public string? Name { get; set; }
        public string? IfscAlias { get; set; }
        public string? IfscGlobal { get; set; }
        public int? NeftEnabled { get; set; }
        public string? NeftFailureRate { get; set; }
        public int? ImpsEnabled { get; set; }
        public string? ImpsFailureRate { get; set; }
        public int? UpiEnabled { get; set; }
        public string? UpiFailureRate { get; set; }
    }


}
