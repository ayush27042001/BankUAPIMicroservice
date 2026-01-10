using BankUAPI.Application.Interface;
using BankUAPI.Application.Interface.DMT.Provider;
using BankUAPI.SharedKernel.Request.DMT.InstantPay;
using BankUAPI.SharedKernel.Response.DMT.InstantPay;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankUAPI.Application.Implementation.DMT.InstantPay
{
    public class InstantPayProvider : IDmtProvider
    {
        private readonly IInstantPayClient _client;
        private readonly IUserRepository _UserRepo;

        public InstantPayProvider(IInstantPayClient client, IUserRepository userRepo)
        {
            _client = client;
            _UserRepo = userRepo;
        }

        public async Task<RemitterProfileResponse> GetRemitterProfileAsync(RemitterProfileRequest request, CancellationToken ct)
        {
            string? outletId = await _UserRepo.GetOutletIdAsync(
                Convert.ToInt32(request.UserId),
                ct
            );

            if (string.IsNullOrWhiteSpace(outletId))
            {
                return new RemitterProfileResponse
                {
                    success = false,
                    StatusCode = "401",
                    Status = "OutletId not found, Please complete your onboarding!"
                };
            }

            return await _client.GetRemitterProfileAsync(
                request,
                outletId: outletId,
                endpointIp: request.EndpointIp,
                ct
            );
        }

    }
}
