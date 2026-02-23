using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.AppSettingModel
{
    public sealed class CommissionRate
    {
        public decimal? RetailerValue { get; init; }
        public int? RetailerType { get; init; }     // 1 = Flat, 2 = Percent

        public decimal? DistributorValue { get; init; }
        public int? DistributorType { get; init; }  // 1 = Flat, 2 = Percent

        public decimal? AdminValue { get; init; }
        public int? AdminType { get; init; }
    }
}
