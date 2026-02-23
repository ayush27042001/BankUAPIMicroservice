using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Infrastructure.Sql.Entities
{
    public class CommissionDistribution
    {
        [Key]
        public int CommissionDistributionId { get; set; }
        public int CommissionSlabId { get; set; }
        public string UserType { get; set; } = null!;
        public int CommissionType { get; set; } // 1=FLAT, 2=PERCENT
        public decimal CommissionValue { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now; 
        public CommissionSlab CommissionSlab { get; set; } = null!;
    }
}
