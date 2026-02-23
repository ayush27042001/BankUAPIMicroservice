using BankUAPI.Application.Implementation.Commision.CommisionHeader;
using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.Commision.CommisionHeader;
using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Response.CommonResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankU.API.Controllers
{
    [ApiController]
    [Route("api/admin/commission-headers")]
    public class CommissionHeaderController : ControllerBase
    { 
        private readonly ICommisionHeaderOps _commisionHeader;
        public CommissionHeaderController(ICommisionHeaderOps comm)
        {
            _commisionHeader= comm;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int planId, int page = 1, int pageSize = 10)
        {
            var result = await _commisionHeader.GetAllCommisionHeader(planId, page, pageSize);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHeaderDTO dto)
        {
            try
            {
                var header = await _commisionHeader.Create(dto);
                return Ok(header);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommissionHeader(int id, UpdateHeaderDTO dto)
        {
            try
            {
                var header = await _commisionHeader.Update(id, dto);
                return Ok(header);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommissionHeader(int id)
        {
            try
            {
                await _commisionHeader.DeleteAsync(id);
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
