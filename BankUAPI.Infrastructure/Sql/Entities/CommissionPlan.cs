using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class CommissionPlan
    {
        [Key]
        public int PlanId { get; set; }
        public string? PlanName { get; set; }
        public bool? IsActive { get; set; }
        public ICollection<CommissionHeader> CommissionHeaders { get; set; } = new List<CommissionHeader>();
    }
}
