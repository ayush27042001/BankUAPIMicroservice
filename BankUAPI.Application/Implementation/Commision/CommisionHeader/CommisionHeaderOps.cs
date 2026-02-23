using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Response.CommisionHeaderRes;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Commision.CommisionHeader
{
    public class CommisionHeaderOps : ICommisionHeaderOps
    {
        private readonly AppDbContext _dbContext;

        public CommisionHeaderOps(AppDbContext db)
        {
            _dbContext = db;
        }

        public async Task<PagedResult<CommissionHeaderDto>> GetAllCommisionHeader(int planId = 1, int page = 1, int pageSize = 10)
        {
            var query = _dbContext.CommissionHeader.Where(h => h.PlanId == planId).AsNoTracking();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(h => h.CommissionRuleId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new CommissionHeaderDto
                {
                    PlanName = h.CommissionPlan.PlanName ??"",
                    CommissionRuleId = h.CommissionRuleId,
                    ServiceId = h.ServiceId,
                    ProviderId = h.ProviderId,
                    OperatorId = h.OperatorId,
                    IsActive = h.IsActive,
                    CreatedOn = h.CreatedOn,

                    Slabs = h.Slabs.Select(s => new CommissionSlabDto
                    {
                        CommissionSlabId = s.CommissionSlabId,
                        FromAmount = s.FromAmount,
                        ToAmount = s.ToAmount,

                        Distributions = s.Distributions.Select(d => new CommissionDistributionDto
                        {
                            CommissionDistributionId = d.CommissionDistributionId,
                            UserType = d.UserType,
                            CommissionType = d.CommissionType,
                            CommissionValue = d.CommissionValue
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResult<CommissionHeaderDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<CommissionHeader> Create(CreateHeaderDTO dto)
        {
            bool exists = await _dbContext.CommissionHeader.AnyAsync(h =>
                h.ServiceId == dto.ServiceId && h.PlanId == dto.PlanId && h.ProviderId == dto.ProviderId && h.OperatorId == dto.OperatorId);

            if (exists)
            {
                throw new InvalidOperationException("Rule already exists with this combination.");
            }

            var header = new CommissionHeader
            {
                PlanId = dto.PlanId,
                ServiceId = dto.ServiceId,
                ProviderId = dto.ProviderId,
                OperatorId = dto.OperatorId
            };
            _dbContext.CommissionHeader.Add(header);
            await _dbContext.SaveChangesAsync();
            return header;
        }

        public async Task<CommissionHeader> Update(int id, UpdateHeaderDTO dto)
        {
            var header = await _dbContext.CommissionHeader.FindAsync(id);

            if (header == null)
                throw new KeyNotFoundException($"CommissionHeader with id {id} not found.");

            // Update the properties
            header.IsActive = dto.IsActive;

            await _dbContext.SaveChangesAsync();

            return header;
        }

        public async Task DeleteAsync(int id)
        {
            var header = await _dbContext.CommissionHeader.FindAsync(id);

            if (header == null)
            {
                throw new KeyNotFoundException($"CommissionHeader with id {id} does not exist.");
            }

            _dbContext.CommissionHeader.Remove(header);
            await _dbContext.SaveChangesAsync();
        }


    }
}
