using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.AddFund
{
    public class AddFundRequest
    {
        public string UserId { get; set; }
        public string Apiversion { get; set; }
        public string Amt { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string RedirectUrl { get; set; }
        public string OrderId { get; set; }
        public string IdempotencyKey { get; set; }
    }
}
