using BankUAPI.SharedKernel.Request.AddFund;
using BankUAPI.SharedKernel.Request.Ticket;
using BankUAPI.SharedKernel.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface.Ticket
{
    public interface ITicketService
    {
        Task<TicketResponse> CreateTicketAsync(TicketModel model, IFormFile file, CancellationToken cn);
    }
}
