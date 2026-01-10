using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.Application.Interface.Commision.CommissionSlabs;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Request.Commission.Slabs;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/admin/commission-slabs")]
    public class CommissionSlabController : ControllerBase
    {
        private readonly ICommisionSlabsOps _commisionSlabs;
        public CommissionSlabController(ICommisionSlabsOps comm)
        {
            _commisionSlabs = comm;
        }

        [HttpGet("GetSlab")]
        public async Task<IActionResult> GetSlab(int ruleId)
        {
            var data = await _commisionSlabs.GetSlab(ruleId);
            return Ok(data);
        }

        [HttpGet("{ruleId}")]
        public async Task<IActionResult> GetSlabs(int ruleId, int page = 1, int pageSize = 10)
        {
            var data= await _commisionSlabs.GetSlabs(ruleId, page, pageSize);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlab(CreateSlabDTO dto)
        {
            try
            {
                var slab = await _commisionSlabs.Create(dto);
                return Ok(slab);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSlabDTO dto)
        {
            try
            {
                var slab = await _commisionSlabs.UpdateAsync(id, dto);
                return Ok(slab);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _commisionSlabs.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
