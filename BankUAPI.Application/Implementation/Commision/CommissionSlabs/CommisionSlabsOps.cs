using BankUAPI.Application.Interface.Commision.CommissionSlabs;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.Slabs;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Commision.CommissionSlabs
{
    public class CommisionSlabsOps : ICommisionSlabsOps
    {
        private readonly AppDbContext _dbContext;

        public CommisionSlabsOps(AppDbContext db)
        {
            _dbContext = db;
        }

        public async Task<PagedResult<CommissionSlab>> GetSlabs(int ruleId, int page = 1, int pageSize = 10)
        {
            var query = _dbContext.CommissionSlabs.Where(s => s.CommissionRuleId == ruleId);
            var total = await query.CountAsync();
            var items = await query.OrderBy(s => s.FromAmount)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(s => s.Distributions)
                .ToListAsync();
            return (new PagedResult<CommissionSlab> { Items = items, TotalCount = total, Page = page, PageSize = pageSize });
        }

        public async Task<PagedResult<CommissionSlab>> GetSlab(int ruleId)
        {
            var query = _dbContext.CommissionSlabs.Where(s => s.CommissionRuleId == ruleId);
            var total = await query.CountAsync();
            var items = await query.OrderBy(s => s.FromAmount)
                .Include(s => s.Distributions)
                .ToListAsync();
            return (new PagedResult<CommissionSlab> { Items = items, TotalCount = total});
        }

        public async Task<CommissionSlab> Create(CreateSlabDTO dto)
        {
            
            bool overlap = await _dbContext.CommissionSlabs.AnyAsync(s =>
                s.CommissionRuleId == dto.CommissionRuleId &&
                ((dto.FromAmount >= s.FromAmount && dto.FromAmount <= s.ToAmount) ||
                 (dto.ToAmount >= s.FromAmount && dto.ToAmount <= s.ToAmount))
            );

            if (overlap)
                throw new InvalidOperationException("Slab overlaps with existing slabs.");


            var slab = new CommissionSlab
            {
                CommissionRuleId = dto.CommissionRuleId,
                FromAmount = dto.FromAmount,
                ToAmount = dto.ToAmount
            };

            _dbContext.CommissionSlabs.Add(slab);
            await _dbContext.SaveChangesAsync();

            return slab;
        }

        public async Task<CommissionSlab> UpdateAsync(int id, UpdateSlabDTO dto)
        {
            var slab = await _dbContext.CommissionSlabs.FindAsync(id);
            if (slab == null)
                throw new KeyNotFoundException($"CommissionSlab with id {id} not found.");

            bool overlap = await _dbContext.CommissionSlabs.AnyAsync(s =>
                s.CommissionRuleId == slab.CommissionRuleId &&
                s.CommissionSlabId != id &&
                ((dto.FromAmount >= s.FromAmount && dto.FromAmount <= s.ToAmount) ||
                 (dto.ToAmount >= s.FromAmount && dto.ToAmount <= s.ToAmount))
            );

            if (overlap)
                throw new InvalidOperationException("Updated slab overlaps with existing slabs.");

            slab.FromAmount = dto.FromAmount;
            slab.ToAmount = dto.ToAmount;

            await _dbContext.SaveChangesAsync();

            return slab;
        }

        public async Task DeleteAsync(int id)
        {
            var slab = await _dbContext.CommissionSlabs.FindAsync(id);
            if (slab == null)
                throw new KeyNotFoundException($"CommissionSlab with id {id} not found.");

            _dbContext.CommissionSlabs.Remove(slab);
            await _dbContext.SaveChangesAsync();
        }

    }
}
