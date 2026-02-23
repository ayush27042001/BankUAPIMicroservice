using BankUAPI.Application.Interface.Commision.CommisionDistribution;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.AppSettingModel;
using BankUAPI.SharedKernel.Request.Commission.SlabsDistribution;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.Commision.CommisionDistribution
{
    public class CommissionDistributionService : ICommissionDistributionService
    {
        private readonly AppDbContext _dbContext;

        public CommissionDistributionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<CommissionDistribution>> GetDistributionsAsync(int slabId, int page = 1, int pageSize = 10)
        {
            var query = _dbContext.CommissionDistribution
                .Where(d => d.CommissionSlabId == slabId);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.UserType)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<CommissionDistribution>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<CommissionDistribution> CreateAsync(CreateDistributionDTO dto)
        {
            bool exists = await _dbContext.CommissionDistribution
                .AnyAsync(d => d.CommissionSlabId == dto.CommissionSlabId &&
                               d.UserType == dto.UserType);

            if (exists)
                throw new InvalidOperationException("Distribution for this user type already exists.");

            var dist = new CommissionDistribution
            {
                CommissionSlabId = dto.CommissionSlabId,
                UserType = dto.UserType,
                CommissionType = dto.CommissionType,
                CommissionValue = dto.CommissionValue
            };

            _dbContext.CommissionDistribution.Add(dist);
            await _dbContext.SaveChangesAsync();

            return dist;
        }

        public async Task<CommissionDistribution> UpdateAsync(int id, UpdateDistributionDTO dto)
        {
            var dist = await _dbContext.CommissionDistribution.FindAsync(id);
            if (dist == null)
                throw new KeyNotFoundException($"Distribution with id {id} not found.");

            dist.CommissionType = dto.CommissionType;
            dist.CommissionValue = dto.CommissionValue;

            await _dbContext.SaveChangesAsync();

            return dist;
        }

        public async Task DeleteAsync(int id)
        {
            var dist = await _dbContext.CommissionDistribution.FindAsync(id);
            if (dist == null)
                throw new KeyNotFoundException($"Distribution with id {id} not found.");

            _dbContext.CommissionDistribution.Remove(dist);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<CommissionRate> GetDmtCommissionAsync(string serviceId, string providerId, string? operatorId, decimal txnAmount, int PlanId=1)
        {
            var rate = await _dbContext.CommissionHeader
                .AsNoTracking()
                .Where(h =>
                    h.ServiceId == serviceId &&
                    h.ProviderId == providerId &&
                    h.OperatorId == operatorId &&
                    h.PlanId == PlanId &&
                    h.IsActive)
                .SelectMany(h => h.Slabs
                    .Where(s => txnAmount >= s.FromAmount && txnAmount <= s.ToAmount)
                    .Select(s => new CommissionRate
                    {
                        RetailerValue = s.Distributions
                            .Where(d => d.UserType == "Retailer")
                            .Select(d => d.CommissionValue)
                            .First(),

                        RetailerType = s.Distributions
                            .Where(d => d.UserType == "Retailer")
                            .Select(d => d.CommissionType)
                            .First(),

                        DistributorValue = s.Distributions
                            .Where(d => d.UserType == "AD")
                            .Select(d => d.CommissionValue)
                            .First(),

                        DistributorType = s.Distributions
                            .Where(d => d.UserType == "AD")
                            .Select(d => d.CommissionType)
                            .First(),

                        AdminValue = s.Distributions
                            .Where(d => d.UserType == "WL")
                            .Select(d => d.CommissionValue)
                            .First(),

                        AdminType = s.Distributions
                            .Where(d => d.UserType == "WL")
                            .Select(d => d.CommissionType)
                            .First()
                    }))
                .FirstOrDefaultAsync();

            if (rate == null)
                throw new InvalidOperationException("DMT commission slab not configured");

            return rate;
        }

    }
}
