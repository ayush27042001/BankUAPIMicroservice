using BankUAPI.SharedKernel.Response.AddFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.AddFund
{
    public class LoginModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public AddFundResult Data { get; set; }
    }
    public class AddFundResult
    {
        public List<AddFundResponse> Transactions { get; set; }
    }

}
