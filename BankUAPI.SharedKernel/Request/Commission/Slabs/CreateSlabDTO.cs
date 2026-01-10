using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Commission.Slabs
{
    public class CreateSlabDTO
    {
        public int CommissionRuleId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
    }

    public class UpdateSlabDTO
    {
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
    }
}
