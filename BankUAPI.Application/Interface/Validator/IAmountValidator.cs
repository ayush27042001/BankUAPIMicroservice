using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Validator
{
    public interface IAmountValidator
    {
        IDFCApiResponse<decimal> Validate(string amount);
    }
}
