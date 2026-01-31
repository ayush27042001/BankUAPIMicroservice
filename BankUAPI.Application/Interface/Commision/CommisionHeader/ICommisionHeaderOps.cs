using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Commission.CommisionHeader;
using BankUAPI.SharedKernel.Response.CommisionHeaderRes;
using BankUAPI.SharedKernel.Response.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Commision.CommisionHeader
{
    public interface ICommisionHeaderOps
    {
        Task<PagedResult<CommissionHeaderDto>> GetAllCommisionHeader(int page = 1, int pageSize = 10);
        Task<CommissionHeader> Create(CreateHeaderDTO dto);
        Task<CommissionHeader> Update(int id, UpdateHeaderDTO dto);
        Task DeleteAsync(int id);
    }
}
