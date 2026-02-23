using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Commission.CommisionHeader
{
    public class CreateHeaderDTO
    {
        public int PlanId { get; set; }
        public string ServiceId { get; set; }
        public string ProviderId { get; set; }
        public string? OperatorId { get; set; } = null;
    }

    public class UpdateHeaderDTO
    {
        public bool IsActive { get; set; }
    }
}
