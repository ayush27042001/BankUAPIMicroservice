using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUAPI.SharedKernel.Request.Payout.IDFC
{
    public class InitiateAuthGenericFundTransferAPIReq
    {
        public string? tellerBranch { get; set; }
        public string? tellerID { get; set; }
        public string? transactionID { get; set; }
        public string? debitAccountNumber { get; set; }
        public string? creditAccountNumber { get; set; }
        public string? remitterName { get; set; }
        public string? amount { get; set; }
        public string? currency { get; set; }
        public string? transactionType { get; set; }
        public string? paymentDescription { get; set; }
        public string? beneficiaryIFSC { get; set; }
        public string? beneficiaryName { get; set; }
        public string? beneficiaryAddress { get; set; }
        public string? emailId { get; set; }
        public string? mobileNo { get; set; }
    }

}
