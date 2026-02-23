using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.CommisionHeaderRes
{
    public class CommissionHeaderDto
    {
        public int CommissionRuleId { get; set; }
        public string ServiceId { get; set; }
        public string ProviderId { get; set; }
        public string? OperatorId { get; set; }
        public string? PlanName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<CommissionSlabDto> Slabs { get; set; } = new();
    }

    public class CommissionSlabDto
    {
        public int CommissionSlabId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }

        public List<CommissionDistributionDto> Distributions { get; set; } = new();
    }

    public class CommissionDistributionDto
    {
        public int CommissionDistributionId { get; set; }
        public string UserType { get; set; }
        public int CommissionType { get; set; }
        public decimal CommissionValue { get; set; }
    }



}
