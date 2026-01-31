using BankUAPI.SharedKernel.Response.Payout.IDFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Validator
{
    public sealed class AmountValidator : IAmountValidator
    {
        public IDFCApiResponse<decimal> Validate(string amount)
        {
            if (!decimal.TryParse(amount, out var value))
                return IDFCApiResponse<decimal>.Fail("Invalid transfer amount");

            if (value < 100 || value > 25000)
                return IDFCApiResponse<decimal>.Fail("Amount must be between 100 and 25000");

            return IDFCApiResponse<decimal>.Ok(value);
        }
    }

}
