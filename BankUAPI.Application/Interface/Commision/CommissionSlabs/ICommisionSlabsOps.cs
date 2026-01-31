using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.Slabs;
using BankUAPI.SharedKernel.Response.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Commision.CommissionSlabs
{
    public interface ICommisionSlabsOps
    {
        Task<PagedResult<CommissionSlab>> GetSlabs(int ruleId, int page = 1, int pageSize = 10);

        Task<CommissionSlab> Create(CreateSlabDTO dto);

        Task<CommissionSlab> UpdateAsync(int id, UpdateSlabDTO dto);

        Task DeleteAsync(int id);
        Task<PagedResult<CommissionSlab>> GetSlab(int ruleId);


    }
}
