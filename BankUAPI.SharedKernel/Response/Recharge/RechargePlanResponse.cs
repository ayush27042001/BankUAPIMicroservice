using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Recharge
{
    public class RechargePlanResponse
    {
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
        public List<RechargePlanDetails> Plans { get; set; } = new();
    }

    public class RechargePlanDetails
    {
        public string Operator { get; set; } = "";
        public string Circle { get; set; } = "";
        public string Amount { get; set; } = "";
        public string Validity { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
    }
    public class MplanApiResponse
    {
        public int Status { get; set; }

        public Dictionary<string, List<MplanPlan>> Records { get; set; } = new();
    }

    public class MplanRecords
    {
        public List<MplanPlan> Data { get; set; } = new(); 
        public string Msg { get; set; } = "";
    }

    public class MplanPlan
    {
        public string Rs { get; set; } = "";
        public string Desc { get; set; } = "";
        public string Validity { get; set; } = "";
        public string Last_Update { get; set; } = "";
    }
}
