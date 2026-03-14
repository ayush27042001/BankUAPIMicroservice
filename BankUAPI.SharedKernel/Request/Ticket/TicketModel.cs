using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Ticket
{
    public class TicketModel
    {
        public int UserId { get; set; }

        public string TransactionId { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
