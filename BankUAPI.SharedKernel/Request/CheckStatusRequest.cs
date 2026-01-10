using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed class CheckStatusRequest
    {
        public string UserId { get; init; }
        public string SpKey { get; init; } 
    }
}
