using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public sealed class aepsresponse
    {
        public string status { get; init; } = string.Empty;
        public string orderstatus { get; init; } = string.Empty;
        public string txntype { get; init; } = string.Empty;

        public string orderamount { get; init; } = "0";
        public string acamount { get; init; } = "0";

        public string agentid { get; init; } = string.Empty;
        public string? bankrefno { get; init; } = string.Empty;

        public object? xmllist { get; init; } 

        public string OldBalance { get; init; } = "0";
        public string NewBalance { get; init; } = "0";
        public string Commission { get; init; } = "0";
    }

    public sealed class LoginModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<aepsresponse> Data { get; set; } = new();

        public static LoginModel Fail(string message) =>
            new() { Status = "FAILED", Message = message };
    }

    public sealed class UserOutlet
    {
        public string UserId { get; init; }
        public string OutletId { get; init; }
    }


}
