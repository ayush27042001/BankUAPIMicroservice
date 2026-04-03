using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.Recharge
{
    public class FetchHlrResponse
    {
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";

        public string Mobile { get; set; } = "";
        public string Operator { get; set; } = "";
        public string OperatorCode { get; set; } = "";
        public string Circle { get; set; } = "";
        public string CircleCode { get; set; } = "";
    }
    public class PlanApiHlrResponse
    {
        public string ERROR { get; set; } = "";
        public string STATUS { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Operator { get; set; } = "";
        public string OpCode { get; set; } = "";
        public string Circle { get; set; } = "";
        public string CircleCode { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
