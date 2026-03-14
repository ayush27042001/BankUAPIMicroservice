using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.KYC
{
    public class GetKycResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string KycStatus { get; set; }

        public List<KycDocument> Documents { get; set; }
    }

    public class KycDocument
    {
        public string DocumentType { get; set; }
        public string FileUrl { get; set; }
    }
}
