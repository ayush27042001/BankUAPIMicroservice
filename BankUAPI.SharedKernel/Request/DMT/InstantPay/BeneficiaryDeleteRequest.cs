using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.DMT.InstantPay
{
    public class BeneficiaryDeleteRequest
    {
        public string? remitterMobileNumber { get; set; } = "";
        public string? beneficiaryId { get; set; } = "";
        public string? EndpointIp { get; set; } = "";
        public string? UserId { get; set; } = "";
    }
}
