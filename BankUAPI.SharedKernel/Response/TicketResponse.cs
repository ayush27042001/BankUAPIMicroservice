using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public class TicketResponse    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
    }
}
