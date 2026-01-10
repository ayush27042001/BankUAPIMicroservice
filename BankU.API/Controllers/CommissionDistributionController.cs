using BankUAPI.Application.Interface.Commision.CommisionDistribution;
using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.SharedKernel.Request.Commission.SlabsDistribution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [Route("api/[controller]")]
    [Route("api/admin/commission-distributions")]
    public class CommissionDistributionController : ControllerBase
    {
        private readonly ICommissionDistributionService _commisionDistribution;
        public CommissionDistributionController(ICommissionDistributionService comm)
        {
            _commisionDistribution = comm;
        }

        [HttpGet("{slabId}")]
        public async Task<IActionResult> GetDistributions(int slabId, int page = 1, int pageSize = 10)
        {
            var result = await _commisionDistribution.GetDistributionsAsync(slabId, page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistributionDTO dto)
        {
            try
            {
                var dist = await _commisionDistribution.CreateAsync(dto);
                return Ok(dist);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDistributionDTO dto)
        {
            try
            {
                var dist = await _commisionDistribution.UpdateAsync(id, dto);
                return Ok(dist);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commisionDistribution.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
