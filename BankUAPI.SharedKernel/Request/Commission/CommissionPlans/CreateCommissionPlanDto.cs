using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Commission.CommissionPlans
{
    public class CreateCommissionPlanDto
    {
        public string? PlanName { get; set; }
        public bool? IsActive { get; set; }
    }

}
