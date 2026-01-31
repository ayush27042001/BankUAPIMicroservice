using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.DMT.InstantPay
{
    public class CommissionResponse
    {
        public decimal? WalletAmount { get;set; }
        public decimal? RetailerCommissionCharge { get;set; }
        public decimal? DistributerCommision { get;set; }
        public decimal? AdminCommision { get;set; }
        public bool? success { get;set; }
    }
}
