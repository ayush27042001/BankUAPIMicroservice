using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response
{
    public class AgreementResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<AgreementData> Data { get; set; }
    }

    public class AgreementData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AgreementId { get; set; }
        public string AgreementType { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
    }
}
