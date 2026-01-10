using BankUAPI.Application.Implementation.DMT.InstantPay;
using BankUAPI.Application.Interface.DMT.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.Application.Factory
{
    public sealed class DmtProviderFactory
    {
        private readonly IServiceProvider _sp;

        public DmtProviderFactory(IServiceProvider sp)
        {
            _sp = sp;
        }

        public IDmtProvider Get(string provider)
            => provider.ToLower() switch
            {
                "instantpay" => _sp.GetRequiredService<InstantPayProvider>(),
                _ => throw new NotSupportedException("Provider not supported")
            };
    }

}
