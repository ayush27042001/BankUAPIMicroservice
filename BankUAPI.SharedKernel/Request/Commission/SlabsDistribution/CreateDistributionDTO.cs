using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Commission.SlabsDistribution
{
    public class CreateDistributionDTO
    {
        public int CommissionSlabId { get; set; }
        public string UserType { get; set; } = null!;
        public int CommissionType { get; set; }
        public decimal CommissionValue { get; set; }
    }

    public class UpdateDistributionDTO
    {
        public int CommissionType { get; set; }
        public decimal CommissionValue { get; set; }
    }
}
