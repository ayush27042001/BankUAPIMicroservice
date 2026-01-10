using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.SlabsDistribution;
using BankUAPI.SharedKernel.Response.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Commision.CommisionDistribution
{
    public interface ICommissionDistributionService
    {
        Task<PagedResult<CommissionDistribution>> GetDistributionsAsync(int slabId, int page = 1, int pageSize = 10);
        Task<CommissionDistribution> CreateAsync(CreateDistributionDTO dto);
        Task<CommissionDistribution> UpdateAsync(int id, UpdateDistributionDTO dto);
        Task DeleteAsync(int id);

    }
}
