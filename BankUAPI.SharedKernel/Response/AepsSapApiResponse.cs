using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public sealed class AepsSapApiResponse
    {
        public string statuscode { get; set; }
        public string status { get; set; }
        public string orderid { get; set; }
        public string ipay_uuid { get; set; }
        public SapData data { get; set; }

        public sealed class SapData
        {
            public string operatorId { get; set; }
            public string bankAccountBalance { get; set; }
            public object miniStatement { get; set; }
        }
    }

}
