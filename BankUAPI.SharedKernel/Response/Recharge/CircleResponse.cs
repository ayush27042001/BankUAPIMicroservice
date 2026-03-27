using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Recharge
{
    public class CircleResponse
    {
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
        public List<CircleDetails> Circles { get; set; } = new();
    }

    public class CircleDetails
    {
        public string CircleCode { get; set; } = "";
        public string CircleName { get; set; } = "";
    }
}
