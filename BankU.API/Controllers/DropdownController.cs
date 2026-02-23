using BankUAPI.Infrastructure.Sql.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/dropdowns")]
    public class DropdownController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DropdownController(AppDbContext db) => _db = db;

        // 1️⃣ Service Drop-Down
        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _db.BankUservices
                .Where(s => s.Status == "Active")
                .Select(s => new { s.Id, s.ServiceName })
                .OrderBy(s => s.ServiceName)
                .ToListAsync();
            return Ok(services);
        }

        // 2️⃣ Provider Drop-Down (optionally filtered by Service)
        [HttpGet("providers")]
        public async Task<IActionResult> GetProviders([FromQuery] string? serviceCode = null)
        {
            var query = _db.MASTER_PROVIDER.Where(p => p.IsEnabled);

            if (!string.IsNullOrEmpty(serviceCode))
            {
                var validProviders = await _db.SERVICE_PROVIDER
                    .Where(sp => sp.ServiceCode == serviceCode && sp.IsEnabled)
                    .Select(sp => sp.ProviderCode)
                    .ToListAsync();

                query = query.Where(p => validProviders.Contains(p.ProviderCode));
            }

            var providers = await query.Select(p => new { p.ProviderId, p.ProviderName, p.ProviderCode }).ToListAsync();
            return Ok(providers);
        }

        // 3️⃣ Operator Drop-Down (optionally filtered by Service)
        [HttpGet("operators")]
        public async Task<IActionResult> GetOperators([FromQuery] string? serviceName = null)
        {
            var query = _db.Tbloperators.AsQueryable();

            if (!string.IsNullOrEmpty(serviceName))
                query = query.Where(o => o.ServiceName == serviceName);

            var operators = await query
                .Where(o => o.Status == "Active")
                .Select(o => new { o.Id, o.OperatorName, o.OperatorImage })
                .ToListAsync();

            return Ok(operators);
        }

        // 4️⃣ Commission Rule Drop-Down (Service + Provider)
        [HttpGet("commission-rules")]
        public async Task<IActionResult> GetCommissionRules([FromQuery] string? serviceId = null, [FromQuery] string? providerId = null, [FromQuery] int? planid = null)
        {
            var query = _db.CommissionHeader.AsQueryable();

            if (serviceId!="") query = query.Where(h => h.ServiceId.Trim().ToLower() == serviceId.Trim().ToLower());
            if (providerId!="") query = query.Where(h => h.ProviderId.Trim().ToLower() == providerId.Trim().ToLower());
            if (planid != null) query = query.Where(h=> h.PlanId == planid);

            var rules = await query.Select(h => new { h.CommissionRuleId, Display = $"Rule {h.CommissionRuleId}" }).ToListAsync();
            return Ok(rules);
        }

        // 5️⃣ Commission Slab Drop-Down (based on Rule)
        [HttpGet("commission-slabs")]
        public async Task<IActionResult> GetCommissionSlabs([FromQuery] int ruleId)
        {
            var slabs = await _db.CommissionSlabs
                .Where(s => s.CommissionRuleId == ruleId)
                .Select(s => new { s.CommissionSlabId, Display = $"{s.FromAmount}-{s.ToAmount}" })
                .OrderBy(s => s.CommissionSlabId)
                .ToListAsync();

            return Ok(slabs);
        }

        [HttpGet("commission-plans")]
        public async Task<IActionResult> GetCommissionPlans()
        {
            var slabs = await _db.CommissionPlan
                .Where(s => s.IsActive == true)
                .Select(s => new { s.PlanId, Display = $"{s.PlanName}" })
                .OrderBy(s => s.PlanId)
                .ToListAsync();

            return Ok(slabs);
        }
    }
}
