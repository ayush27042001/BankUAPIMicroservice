using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Request.Commission.CommissionPlans;
using BankUAPI.SharedKernel.Response.CommisionHeaderRes;
using BankUAPI.SharedKernel.Response.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Commision.CommissionPlans
{
    public interface ICommisionPlanOps
    {
        Task<PagedResult<CommissionPlan>> GetAllCommisionPlans(int page = 1, int pageSize = 10);
        Task<CommissionPlan> Create(CreateCommissionPlanDto dto);
        Task<CommissionPlan> Update(int id, CreateCommissionPlanDto dto);
        Task DeleteAsync(int id);
    }
}
