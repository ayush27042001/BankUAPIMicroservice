using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Bank_Account
{
    public class BankAccountResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string BankName { get; set; }
        public string AccountHolder { get; set; }
    }
}
