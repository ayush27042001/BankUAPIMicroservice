using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.ENUM
{
    public enum OtpVerifyStatus
    {
        Success,
        InvalidOtp,
        Expired,
        NotFound,
        Locked,
        DeviceMismatch
    }
}
