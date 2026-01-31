using BankUAPI.SharedKernel.Request;
using BankUAPI.SharedKernel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Interface
{
    public interface IAepsSignupService
    {
        Task<LoginModel> ExecuteAsync(AepsSignupRequestDto request, CancellationToken ct);
        Task<LoginModel> SignUpValidate(AepsSignupValidateRequestDto request, CancellationToken ct);
    }
}
