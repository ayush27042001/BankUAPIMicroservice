using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.AddFund
{
    public class StatusCheckRequest
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
    }
}
