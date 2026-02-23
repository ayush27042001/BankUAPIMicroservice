using BankUAPI.Application.Interface.Commision.CommissionPlans;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Request.Commission.CommissionPlans;
using BankUAPI.SharedKernel.Response.CommisionHeaderRes;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Commision.CommissionPlans
{
    public class CommisionPlanOps : ICommisionPlanOps
    {
        private readonly AppDbContext _dbContext;

        public CommisionPlanOps(AppDbContext db)
        {
            _dbContext = db;
        }

        public async Task<PagedResult<CommissionPlan>> GetAllCommisionPlans(int page = 1, int pageSize = 10)
        {
            var query = _dbContext.CommissionPlan.AsNoTracking();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.PlanId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CommissionPlan>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<CommissionPlan> Create(CreateCommissionPlanDto dto)
        {
            bool exists = await _dbContext.CommissionPlan.AnyAsync(p => p.PlanName.ToLower() == dto.PlanName.ToLower());

            if (exists)
            {
                throw new InvalidOperationException("Plan already exists.");
            }

            var plan = new CommissionPlan
            {
                PlanName = dto.PlanName,
                IsActive = dto.IsActive
            };

            _dbContext.CommissionPlan.Add(plan);
            await _dbContext.SaveChangesAsync();

            return plan;
        }

        public async Task<CommissionPlan> Update(int id, CreateCommissionPlanDto dto)
        {
            var plan = await _dbContext.CommissionPlan.FindAsync(id);

            bool duplicate = await _dbContext.CommissionPlan.AnyAsync(p => p.PlanId != id &&
                   p.PlanName.ToLower() == dto.PlanName.ToLower());

            if (duplicate)
            {
                throw new InvalidOperationException("Another plan with same name already exists.");
            }

            if (plan == null)
            {
                throw new KeyNotFoundException($"CommissionPlan with id {id} not found.");
            }

            plan.PlanName = dto.PlanName;
            plan.IsActive = dto.IsActive;

            await _dbContext.SaveChangesAsync();
            return plan;
        }

        public async Task DeleteAsync(int id)
        {
            var plan = await _dbContext.CommissionPlan.FindAsync(id);

            if (plan == null)
                throw new KeyNotFoundException($"CommissionPlan with id {id} does not exist.");

            _dbContext.CommissionPlan.Remove(plan);
            await _dbContext.SaveChangesAsync();
        }

    }
}
