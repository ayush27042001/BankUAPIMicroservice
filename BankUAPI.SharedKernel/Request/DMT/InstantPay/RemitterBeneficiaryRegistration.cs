using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class RemitterBeneficiaryRegistration
    {
        public string beneficiaryMobileNumber { get; set; } = default!;
        public string remitterMobileNumber { get; set; } = default!;
        public string accountNumber { get; set; } = default!;
        public string ifsc { get; set; } = default!;
        public string bankId { get; set; } = default!;
        public string name { get; set; } = default!;
        public string? UserId { get; set; }
        public string? EndpointIp { get; set; } = "0.0";
    }
}
