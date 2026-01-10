using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.AppSettingModel
{
    public sealed class InstantPayOptions
    {
        public string BaseUrl { get; init; }
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
        public string AuthCode { get; init; }
    }

}
