using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request
{
    public class ResetMpin
    {
        public string UserId { get; set; }
        public string CurrentMpin { get; set; }
        public string NewMpin { get; set; }
    }
}
