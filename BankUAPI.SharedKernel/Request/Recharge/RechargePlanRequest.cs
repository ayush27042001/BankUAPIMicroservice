using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Recharge
{
    public class RechargePlanRequest
    {
        public string UserId { get; set; } = "";
        public string OperatorCode { get; set; } = "";
        public string CircleCode { get; set; } = "";
    }
    public class CircleRequest
    {
        public string UserId { get; set; } = "";
    }
}
