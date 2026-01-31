using BankUAPI.Infrastructure.Sql.Entities;
using BankUAPI.SharedKernel.Request.Auth;
using BankUAPI.SharedKernel.Response;
using BankUAPI.SharedKernel.Response.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IUserRepository
    {
        Task<UserOutlet> GetOutletAsync(string userId, CancellationToken ct);
        Task<UserSignupProfile> GetSignupProfileAsync(string userId, CancellationToken ct);
        Task SaveOutletAsync(string userId, string outletId, CancellationToken ct);
        Task<string?> GetOutletIdAsync(int userId, CancellationToken ct);
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
        Task<Registration?> GetUserData(string userId, CancellationToken ct);
    }
}
