using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class CommissionSlab
    {
        [Key]
        public int CommissionSlabId { get; set; }
        public int CommissionRuleId { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public int? PlanId { get; set; }
        [JsonIgnore]
        public CommissionHeader CommissionHeader { get; set; } = null!;
        public CommissionPlan CommissionPlan { get; set; } = null!;
        public List<CommissionDistribution> Distributions { get; set; } = new List<CommissionDistribution>();
    }
}
