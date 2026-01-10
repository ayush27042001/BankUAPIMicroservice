using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed class AepsCheckStatusRequestDto
    {
        public string UserId { get; init; }
        public string SpKey { get; init; }
    }

}
