using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Response.KYC
{
    public class KycResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<KycFileInfo> Files { get; set; }
    }

    public class KycFileInfo
    {
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
    }
}
