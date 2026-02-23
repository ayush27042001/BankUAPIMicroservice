using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class CommissionHeader
    {
        [Key]
        public int CommissionRuleId { get; set; }
        public string ServiceId { get; set; }
        public string ProviderId { get; set; }
        public string? OperatorId { get; set; }
        public bool IsActive { get; set; } = true;
        public int? PlanId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public CommissionPlan CommissionPlan { get; set; } = null!;
        public List<CommissionSlab> Slabs { get; set; } = new();
    }
}
