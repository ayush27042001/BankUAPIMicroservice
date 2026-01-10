using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public sealed record AepsBiometricKycStatusRequest(
    string UserId,
    string SpKey);
}
