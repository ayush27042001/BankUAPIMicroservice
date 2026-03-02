using BankUAPI.SharedKernel.Request.AddFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.AddFund
{
    public interface IAddFundStatusService
    {
        Task<LoginModel> CheckStatus(StatusCheckRequest obj);
    }
}
