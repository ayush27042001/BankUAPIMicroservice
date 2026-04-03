using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Recharge
{
    public class FetchHlrRequest
    {
        public string UserId { get; set; } = "";
        public string MobileNo { get; set; } = "";
    }
}
