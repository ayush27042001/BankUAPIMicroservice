using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Bank_Account
{
    public class AddBankAccountRequest
    {
        public string UserId { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public string Phone { get; set; }
    }
}
