using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public sealed class AepsBapApiResponse
    {
        public string statuscode { get; init; }
        public string status { get; init; }
        public string orderid { get; init; }
        public string ipay_uuid { get; init; }
        public string timestamp { get; init; }
        public string environment { get; init; }
        public BapData data { get; init; }

        public sealed class BapData
        {
            public string openingBalance { get; init; }
            public string ipayId { get; init; }
            public string transactionValue { get; init; }
            public string accountNumber { get; init; }
            public string operatorId { get; init; }
            public string bankAccountBalance { get; init; }
        }
    }

}
