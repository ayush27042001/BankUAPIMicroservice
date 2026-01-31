using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.AppSettingModel.IDFCPayout
{
    public sealed class IdfcBankOptions
    {
        public string ClientId { get; init; } = default!;
        public string Kid { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string TokenUrl { get; init; } = default!;
        public string Source { get; init; } = default!;
        public string DebitAccount { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public string PrivateKeyPath { get; init; } = default!;
        public string[] Scopes { get; init; } = Array.Empty<string>();
        public string FundTransfer { get; init; } = default!;
        public string PaymentStatus { get; init; } = default!;
        public string AccountBalance { get; init; } = default!;
        public string AccountStatement { get; init; } = default!;
        public string BeneValidation { get; init; } = default!;
    }

}
